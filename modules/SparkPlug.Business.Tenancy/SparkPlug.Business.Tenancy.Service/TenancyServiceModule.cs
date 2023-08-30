namespace SparkPlug.Business.Tenancy.Service;

public class TenancyServiceModule : IModule
{
    public void AddModule(IServiceCollection services)
    {
        services.AddScoped<ITenantResolver, TenantService>();
    }

    public void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }
}