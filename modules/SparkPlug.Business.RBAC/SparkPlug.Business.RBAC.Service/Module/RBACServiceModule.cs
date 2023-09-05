namespace SparkPlug.Business.RBAC.Service;

public class RBACServiceModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
       services.AddScoped<PermissionService>();
       services.AddScoped<ResourceService>();
    }

    public void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }
}