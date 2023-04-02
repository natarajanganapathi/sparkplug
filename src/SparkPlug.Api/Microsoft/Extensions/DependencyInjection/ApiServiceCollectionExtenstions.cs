namespace Microsoft.Extensions.DependencyInjection;

public static class ApiServiceCollectionExtenstions
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration, Action<WebApiOptions>? setupAction = default)
    {
        services.AddHttpContextAccessor();
        services.AddOptions();
        services.Configure<WebApiOptions>(configuration.GetSection(WebApiOptions.ConfigPath));
        services.AddScoped(typeof(IRequestContext<>), typeof(RequestContext<>));
        services.AddScoped(typeof(Repository<,>));
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped(typeof(ITenantOptions<>), typeof(TenantOptionsManager<>));
        services.AddScoped(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext?.Items["Tenant"] as ITenant ?? Tenant.Default);

        services.AddMvc(MvcOptions => MvcOptions.Conventions.Add(new GenericControllerRouteConvention()))
                .ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider(typeof(ApiController<,>))))
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });
        // .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);

        if (setupAction != null) services.Configure(setupAction);

        services.AddSwagger();
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
