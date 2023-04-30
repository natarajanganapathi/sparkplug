namespace SparkPlug.Tenancy.Api;

public class TenancyModule : IModule
{
    public void AddModule(IServiceCollection services)
    {
        services.AddScoped<ITenantResolver, TenantService>();
        services.AddScoped(typeof(ITenantOptions<>), typeof(TenantOptions<>));
        services.AddTenancyRouteAttribute();
    }

    public void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
        app.UseTenantResolverMiddleware();
    }
}