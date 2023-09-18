namespace SparkPlug.Business.Tenancy.Api;

public class TenancyApiModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
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