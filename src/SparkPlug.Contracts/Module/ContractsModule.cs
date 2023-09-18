namespace SparkPlug.Contracts;

public class ContractsModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISerializer, NewtonsoftSerializer>();
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }

    public void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
    }
}