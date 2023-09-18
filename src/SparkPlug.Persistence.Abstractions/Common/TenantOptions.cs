namespace SparkPlug.Persistence.Abstractions;

public class TenantOptions<TOptions> : ITenantOptions<TOptions> where TOptions : new()
{
    public TOptions Value { get; }
    public TenantOptions(ITenant tenant)
    {
        Value = GetOptions(tenant.Options);
    }
    private static TOptions GetOptions(IDictionary<string, string?> options)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(options)
            .Build();
        return configuration.GetSection(typeof(TOptions).Name).Get<TOptions>() ?? new();
    }
}