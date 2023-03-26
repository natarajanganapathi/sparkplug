namespace SparkPlug.Persistence.Abstractions;

public interface ITenantResolver
{
    Task<ITenant> ResolveAsync(string? id);
}
