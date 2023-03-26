namespace SparkPlug.Persistence.MongoDb.Configuration;

public class MongoDbOptions
{
    public const string ConfigPath = "SparkPlug:MongoDb";
    public MongoDbOptions()
    {
        ConnectionString = string.Empty;
    }
    [Required]
    public string ConnectionString { get; set; }
}
