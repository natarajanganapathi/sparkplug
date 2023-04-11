namespace SparkPlug.Persistence.EntityFramework;

public class HomeOptions<TOptions> : IOptions<TOptions> where TOptions : class, new()
{
    public TOptions Value { get; }
    public HomeOptions(IOptions<SqlDbOptions> options)
    {
        var value = options.Value;
        var dict = new Dictionary<string, string?>()
        {
            { $"{nameof(DbConfig)}:{nameof(value.ConnectionString)}", value.ConnectionString }
        };
        Value = GetOptions(dict);
    }
    private static TOptions GetOptions(IDictionary<string, string?> options)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(options)
            .Build();
        return configuration.GetSection(typeof(TOptions).Name).Get<TOptions>() ?? new();
    }
}
