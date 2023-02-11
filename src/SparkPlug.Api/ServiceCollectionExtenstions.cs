﻿namespace SparkPlug.Api;

public static class SparkPlugApiServiceCollectionExtenstions
{
    public static IServiceCollection AddSparkPlugApi(this IServiceCollection services, IConfiguration configuration, Action<SparkPlugApiOptions>? setupAction = default)
    {
        // builder.WebHost.UseUrls("http://0.0.0.0:1234/{tenant}/{version}/api");
        services.AddOptions<SparkPlugApiOptions>().BindConfiguration(SparkPlugApiOptions.ConfigPath).ValidateDataAnnotations().ValidateOnStart();
        services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<SparkPlugApiOptions>>().Value);
        // services.Configure<SparkPlugApiOptions>(configuration.GetSection(SparkPlugApiOptions.ConfigPath));
        var config = new SparkPlugApiOptions();
        configuration.Bind(SparkPlugApiOptions.ConfigPath, config);

        services.AddSwagger(config);
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddGenericTypes();
        services.AddMvc(MvcOptions => MvcOptions.Conventions.Add(new GenericControllerRouteConvention()))
        .ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider(typeof(ApiController<,,>))));
        // .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        services.AddControllers(options => options.Filters.Add(new SparkPlugExceptionFilterAttribute()));
        if (setupAction != null) services.Configure(setupAction);
        return services;
    }

    public static void UseSparkPlugApi(this IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        var config = serviceProvider.GetRequiredService<SparkPlugApiOptions>();
        var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
        app.UsePathBase(config.PathBase);
        if (env.IsDevelopment()) { app.UseSwagger(); }
        app.UseGlobalExceptionHandling();
        app.UseTransactionMiddleware();
        if (config.IsMultiTenant) app.UseTenantResolverMiddleware();
        // app.UseHttpsRedirection();
        app.UseSwaggerApi();
        app.UseHealthChecks();
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapGet("/", async context => await context.Response.WriteAsync("Running!...")));
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
