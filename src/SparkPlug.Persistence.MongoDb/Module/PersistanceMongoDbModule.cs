namespace SparkPlug.Persistence.MongoDb;

public class PersistanceMongoDbModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistanceMongoDb(configuration);
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }

    public void UseModule(IApplicationBuilder app)
    {
    }
}
