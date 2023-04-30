namespace SparkPlug.Menu.Api;

public class MenuModule : IModule
{
    public void AddModule(IServiceCollection services)
    {
        services.AddScoped<MenuService>();
    }

    public void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {

    }
}