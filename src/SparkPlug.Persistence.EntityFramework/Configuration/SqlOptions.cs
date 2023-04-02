namespace SparkPlug.Persistence.EntityFramework.Configuration;

public class SqlDbOptions
{
    public const string ConfigPath = "SparkPlug:SqlDb";
    public SqlDbOptions()
    {
        ConnectionString = string.Empty;
    }
    [Required]
    public string ConnectionString { get; set; }
    public Func<string, DbConnection>? GetConnection { get; set; }
}
