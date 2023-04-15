namespace SparkPlug.Sample.Api;

public class TenantResolver : ITenantResolver
{
    private readonly SqlDbOptions _options;
    private readonly IServiceProvider _serviceProvider;
    public TenantResolver(IServiceProvider serviceProvider, IOptions<SqlDbOptions> options)
    {
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }
    public async Task<ITenant> ResolveAsync(string? id)
    {
        return string.IsNullOrWhiteSpace(id) ? GetDefault() : await GetTenantById(id);
    }
    public Tenant GetDefault()
    {
        var dict = new Dictionary<string, string?>()
        {
            { $"{nameof(DbConfig)}:{nameof(_options.ConnectionString)}", _options.ConnectionString }
        };
        return new Tenant() { Options = dict };
    }
    public async Task<Tenant> GetTenantById(string? id)
    {
        if (!Guid.TryParse(id, out Guid guid)) { throw new ArgumentException(new StringBuilder().Append(id).Append(" is not valid tenant id").ToString()); }
        var repo = _serviceProvider.GetRequiredService<HomeRepository<long, TenantDetails>>();
        var tenantDetails = await repo.FindAsync(new QueryRequest()
                .Include(nameof(WebApi.Models.Options))
                .Where(nameof(TenantDetails.TenantId), FieldOperator.Equal, guid), CancellationToken.None);
        // var tenantDetails = await repo.DbSet.Where(x=>x.TenantId == guid).Include(nameof(WebApi.Models.Options)).ToListAsync();
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
    public async Task<IEnumerable<ITenant>> GetAllTenantsAsync()
    {
        var repo = _serviceProvider.GetRequiredService<BaseService<long, TenantDetails>>();
        var result = await repo.FindAsync(new QueryRequest(), CancellationToken.None);
        return result.Select(ToTenant);
    }
}
