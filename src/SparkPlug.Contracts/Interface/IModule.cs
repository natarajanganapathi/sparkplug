namespace SparkPlug.Contracts;

public interface IModule
{
    void AddModule(IServiceCollection services, IConfiguration configuration);
    void UseModule(IApplicationBuilder app);
    void UseMiddelwares(IApplicationBuilder app);
}