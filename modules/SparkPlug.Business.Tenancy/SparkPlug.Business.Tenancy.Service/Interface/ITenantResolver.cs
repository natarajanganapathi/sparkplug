namespace SparkPlug.Business.Tenancy.Service;

public interface ITenantResolver
{
    Task<ITenant> ResolveAsync(string? id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ITenant>> GetAllTenantsAsync(CancellationToken cancellationToken = default);
}
