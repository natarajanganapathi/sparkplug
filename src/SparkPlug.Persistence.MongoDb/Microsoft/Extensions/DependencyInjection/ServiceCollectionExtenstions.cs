namespace Microsoft.Extensions.DependencyInjection;

public static class MongoDbServiceCollectionExtenstions
{
    public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection(MongoDbOptions.ConfigPath));
        services.AddScoped<MongoDbClient>();
        services.AddScoped<IRepositoryProvider, MongoRepositoryProvider>();
        services.AddScoped(typeof(MongoRepository<,>));
        services.AddHealthChecks().AddCheck<MongoDbHealthCheck>("MongoDb", tags: new[] { "mongodb", "all" });
    }

    public static void UseMongoDb(this IApplicationBuilder app, IServiceProvider serviceProvider)
    {
    }
}
