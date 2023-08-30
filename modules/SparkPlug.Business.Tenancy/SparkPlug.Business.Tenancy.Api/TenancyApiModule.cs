namespace SparkPlug.Business.Tenancy.Api;

public class TenancyApiModule : IModule
{
    public void AddModule(IServiceCollection services)
    {
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