namespace SparkPlug.Persistence.EntityFramework;

/// <summary>
/// Represents options for the home-database functionality.
/// </summary>
/// <typeparam name="TOptions">The type of options.</typeparam>
public class HomeOptions<TOptions> : IOptions<TOptions> where TOptions : class, new()
{
    /// <summary>
    /// Gets the value of the options.
    /// </summary>
    public TOptions Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeOptions{TOptions}"/> class.
    /// </summary>
    /// <param name="options">The options containing SQL database configuration.</param>
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
