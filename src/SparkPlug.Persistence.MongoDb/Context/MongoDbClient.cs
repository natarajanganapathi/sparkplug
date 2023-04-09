namespace SparkPlug.Persistence.MongoDb.Context;

public class MongoDbClient
{
    private string DatabaseName { get; set; } = string.Empty;
    public IMongoDatabase Database { get; }
    public MongoDbClient(ITenantOptions<DbConfig> options)
    {
        var _mongoClient = GetClient(options.Value.ConnectionString);
        Database = _mongoClient.GetDatabase(DatabaseName);
    }
    public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
    {
        return Database.GetCollection<TEntity>(collectionName);
    }
    public MongoClient GetClient(string connectionString)
    {
        var url = new MongoUrl(connectionString);
        DatabaseName = url.DatabaseName;
        var settings = MongoClientSettings.FromUrl(url);
        settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
        return new MongoClient(settings);
    }
}
