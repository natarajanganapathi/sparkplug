namespace SparkPlug.Business.Tenancy.Service;

public class TenancyServiceModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITenantResolver, TenantService>();
    }

    public void UseModule(IApplicationBuilder app)
    {
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }
}