namespace SparkPlug.Persistence.EntityFramework;

public class SingleTenant : ITenant
{
    public SingleTenant(IOptions<SqlDbOptions> options)
    {
        var value = options.Value;
        Options = new Dictionary<string, string?>()
        {
            { $"{nameof(DbConfig)}:{nameof(value.ConnectionString)}", value.ConnectionString }
        };
    }
    public string? Id { get; set; }
    public string? Name { get; set; }
    public IDictionary<string, string?> Options { get; set; } = new Dictionary<string, string?>();
}