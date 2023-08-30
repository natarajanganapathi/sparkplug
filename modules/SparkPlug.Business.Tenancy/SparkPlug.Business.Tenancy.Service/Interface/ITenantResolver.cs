namespace SparkPlug.Business.Tenancy.Service;

public interface ITenantResolver
{
    Task<ITenant> ResolveAsync(string? id);
    Task<IEnumerable<ITenant>> GetAllTenantsAsync();
}
