namespace SparkPlug.Business.Menu.Service;

public class MenuServiceModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<MenuService>();
    }

    public void UseModule(IApplicationBuilder app)
    {
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }
}