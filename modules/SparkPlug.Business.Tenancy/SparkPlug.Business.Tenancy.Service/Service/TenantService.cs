namespace SparkPlug.Business.Tenancy.Service;

public class TenantService : BaseService<long, TenantDetails>, ITenantResolver
{
    public TenantService(IServiceProvider serviceProvider) : base(serviceProvider) { }

    public async Task<IEnumerable<ITenant>> GetAllTenantsAsync(CancellationToken cancellationToken = default)
    {
        var result = await FindAsync(new QueryRequest(), cancellationToken);
        return result.Select(ToTenant);
    }
    public async Task<ITenant> ResolveAsync(string? id, CancellationToken cancellationToken = default)
    {
        return await GetByTenantId(id, cancellationToken);
    }
    public async Task<Tenant> GetByTenantId(string? id, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(id, out Guid guid)) { throw new ArgumentException(new StringBuilder().Append(id).Append(" is not valid tenant id").ToString()); }
        var tenantDetails = await FindAsync(new QueryRequest()
                .Include(nameof(TenantDetails.Options))
                .Where(nameof(TenantDetails.TenantId), FieldOperator.Equal, guid), cancellationToken);
        var tenant = tenantDetails?.FirstOrDefault() ?? throw new ArgumentException(new StringBuilder().Append(id).Append(" is not valid tenant id").ToString());
        return ToTenant(tenant);
    }
    private static Tenant ToTenant(TenantDetails tenant)
    {
        var dict = new Dictionary<string, string?>();
        tenant.Options.ForEach(kv => dict[kv.Key] = kv.Value);
        return new Tenant() { Id = tenant.Id.ToString(), Name = tenant.Name, Options = dict };
    }
}