namespace SparkPlug.Contracts;

public class ContractsModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddContracts();
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }

    public void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
    }
}