namespace SparkPlug.Business.RBAC.Service;

public class RbacServiceModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<PermissionService>();
        services.AddScoped<ResourceService>();
        services.AddScoped<EndpointService>();
    }

    public void UseModule(IApplicationBuilder app)
    {
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }
}