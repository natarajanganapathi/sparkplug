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
        services.AddCustomModules();
        services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add<ApiExceptionFilterAttribute>();
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
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
        app.UseWhen(context => context.Request.Method != "GET", appBuilder => appBuilder.UseTransactionMiddleware()); // Transaction not required in GET Method

        // Custom Moudeles
        app.UseCustomModules(serviceProvider);
        app.UseCustomModulesMiddelwares();

        app.UseEndpoints(endpoints => endpoints.MapGet("/", async context => await context.Response.WriteAsync("Running!...")));
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
