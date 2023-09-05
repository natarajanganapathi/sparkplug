using Microsoft.AspNetCore.Builder;

namespace SparkPlug.Persistence.MongoDb;

public class PersistanceMongoDbModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection(MongoDbOptions.ConfigPath));
        services.AddScoped<MongoDbClient>();
        services.AddScoped<IRepositoryProvider, MongoRepositoryProvider>();
        services.AddScoped(typeof(MongoRepository<,>));
        services.AddHealthChecks().AddCheck<MongoDbHealthCheck>("MongoDb", tags: new[] { "mongodb", "all" });
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }

    public void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
    }
}
