namespace Microsoft.Extensions.DependencyInjection;

public static class ApiServiceCollectionExtenstions
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddOptions();
        services.Configure<WebApiOptions>(configuration.GetSection(WebApiOptions.ConfigPath));
        services.AddSingleton<ISerializer, NewtonsoftSerializer>();
        services.AddScoped(typeof(IRequestContext<>), typeof(RequestContext<>));
        services.AddScoped(typeof(BaseService<,>));
        services.AddScoped(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext?.Items["Tenant"] as ITenant ?? Tenant.Default);
        services.AddMvc(MvcOptions =>
        {
            IRouteTemplateProvider routeAttribute = new RouteAttribute("{tenant}");
            var isMultiTenant = configuration.GetValue<bool>($"{WebApiOptions.ConfigPath}:{nameof(WebApiOptions.IsMultiTenant)}");
            MvcOptions.Conventions.Add(new GenericControllerRouteConvention(routeAttribute, isMultiTenant));
            if (isMultiTenant) { MvcOptions.UseCentralRoutePrefix(routeAttribute); }
        })
        .ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider(typeof(ApiController<,>))))
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
        // .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);
        services.AddSwagger();
        services.AddHttpClient();
        return services;
    }

    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration, Action<WebApiOptions> setupAction)
    {
        services.AddWebApi(configuration);
        services.Configure(setupAction);
        return services;
    }

    public static void UseWebApi(this IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
        if (env.IsDevelopment()) { app.UseSwaggerApi(); }
        app.UseGlobalExceptionHandling();
        // app UseHttpsRedirection
        app.UseHealthChecks();
        app.UseRouting();
        app.UseTransactionMiddleware();
        app.UseTenantResolverMiddleware();
        app.UseEndpoints(endpoints => endpoints.MapGet("/", async context => await context.Response.WriteAsync("Running!...")));
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
