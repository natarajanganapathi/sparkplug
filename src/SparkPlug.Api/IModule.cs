namespace SparkPlug.Api;

public interface IModule
{
    void AddModule(IServiceCollection services);
    void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider);
    void UseMiddelwares(IApplicationBuilder app);
}