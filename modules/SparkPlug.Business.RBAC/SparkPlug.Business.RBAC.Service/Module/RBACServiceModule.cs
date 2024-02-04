namespace SparkPlug.Business.RBAC.Service;

public class RbacServiceModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
       services.AddScoped<PermissionService>();
       services.AddScoped<ResourceService>();
    }

    public void UseModule(IApplicationBuilder app)
    {
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }
}