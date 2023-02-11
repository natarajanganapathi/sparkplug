namespace SparkPlug.Persistence.MongoDb.Context;

public class MongoDbContext : IMongoDbContext
{
    public IMongoDatabase Database { get; }
    public MongoDbContext(IOptions<SparkPlugMongoDbOptions> options)
    {
        //TODO: Get tenent object from HttpContext option and create mongo client
        var _mongoClient = GetClient(options.Value.ConnectionString);
        Database = _mongoClient.GetDatabase(options.Value.DatabaseName);
    }
    public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
    {
        return Database.GetCollection<TEntity>(collectionName);
    }

    public MongoClient GetClient(string connectionString)
    {
        var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
        settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
        return new MongoClient(settings);
    }
}
