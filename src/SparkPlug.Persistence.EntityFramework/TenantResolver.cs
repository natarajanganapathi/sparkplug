namespace SparkPlug.Persistence.EntityFramework;

public class TenantResolver : ITenantResolver
{
    private readonly IServiceProvider _serviceProvider;
    public TenantResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ITenant> ResolveAsync(string? id)
    {
        var options = _serviceProvider.GetRequiredService<IOptions<SqlDbOptions>>().Value;
        var dict = new Dictionary<string, string?>()
        {
            { $"{nameof(TenantConfig)}:{nameof(options.ConnectionString)}", options.ConnectionString }
        };
        var result = new Tenant() { Options = dict };
        if (!string.IsNullOrWhiteSpace(id))
        {
            if (!Guid.TryParse(id, out Guid guid)) { throw new ArgumentException($"{id} is not valid tenant id"); }
            var repo = _serviceProvider.GetRequiredService<Repository<Guid, TenantDetails>>();
            var tenantDetails = await repo.GetAsync(guid, CancellationToken.None);
            return ToTenant(tenantDetails);
        }
        return result;
    }
    public async Task<IEnumerable<TenantDetails>> GetAllTenantsAsync()
    {
        var repo = _serviceProvider.GetRequiredService<Repository<Guid, TenantDetails>>();
        return await repo.FindAsync(new QueryRequest(), CancellationToken.None);
    }
    private static Tenant ToTenant(TenantDetails tenant)
    {
        var dict = new Dictionary<string, string?>();
        tenant.Options.ForEach(x => dict[x.Key] = x.Value);
        return new Tenant() { Id = tenant.Id.ToString(), Name = tenant.Name, Options = dict };
    }
}
