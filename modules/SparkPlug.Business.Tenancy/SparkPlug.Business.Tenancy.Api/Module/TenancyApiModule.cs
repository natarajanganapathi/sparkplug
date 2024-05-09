namespace SparkPlug.Business.Tenancy.Api;

public class TenancyApiModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTenancyRouteAttribute();
    }

    public void UseModule(IApplicationBuilder app)
    {
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
        app.UseTenantResolverMiddleware();
    }
}