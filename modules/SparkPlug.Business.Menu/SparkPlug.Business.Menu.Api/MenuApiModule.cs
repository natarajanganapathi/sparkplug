namespace SparkPlug.Business.Menu.Api;

public class MenuApiModule : IModule
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