namespace SparkPlug.Persistence.Abstractions;

public interface ITenantOptions<out TOptions> where TOptions : new()
{
    public TOptions Value { get; }
}
