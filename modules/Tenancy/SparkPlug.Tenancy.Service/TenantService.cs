namespace SparkPlug.Tenancy.Service;

public class TenantService : BaseService<long, TenantDetails>, ITenantResolver
{
    public TenantService(IServiceProvider serviceProvider) : base(serviceProvider) { }

    public async Task<IEnumerable<ITenant>> GetAllTenantsAsync()
    {
        var result = await FindAsync(new QueryRequest(), CancellationToken.None);
        return result.Select(ToTenant);
    }
    public async Task<ITenant> ResolveAsync(string? id)
    {
        return await GetByTenantId(id);
    }
    public async Task<Tenant> GetByTenantId(string? id)
    {
        if (!Guid.TryParse(id, out Guid guid)) { throw new ArgumentException(new StringBuilder().Append(id).Append(" is not valid tenant id").ToString()); }
        var tenantDetails = await FindAsync(new QueryRequest()
                .Include(nameof(TenantOption))
                .Where(nameof(TenantDetails.TenantId), FieldOperator.Equal, guid), CancellationToken.None);
        var tenant = tenantDetails?.FirstOrDefault();
        if (tenant == null)
        {
            throw new ArgumentException(new StringBuilder().Append(id).Append(" is not valid tenant id").ToString());
        }
        return ToTenant(tenant);
    }
    private static Tenant ToTenant(TenantDetails tenant)
    {
        var dict = new Dictionary<string, string?>();
        tenant.Options.ForEach(kv => dict[kv.Key] = kv.Value);
        return new Tenant() { Id = tenant.Id.ToString(), Name = tenant.Name, Options = dict };
    }
}