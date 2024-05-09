namespace SparkPlug.Persistence.MongoDb;

public static class PersistanceMongoDbExtensions
{
    public static IServiceCollection AddPersistanceMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection(MongoDbOptions.ConfigPath));
        services.AddScoped<MongoDbClient>();
        services.AddScoped<IRepositoryProvider, MongoRepositoryProvider>();
        services.AddScoped(typeof(MongoRepository<,>));
        services.AddHealthChecks().AddCheck<MongoDbHealthCheck>("MongoDb", tags: ["mongodb", "all"]);
        return services;
    }
}
