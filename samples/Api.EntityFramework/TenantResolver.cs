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
        var dict = new Dictionary<string, string?>()
        {
            { $"{nameof(TenantConfig)}:{nameof(_options.ConnectionString)}", _options.ConnectionString }
        };
        var result = new Tenant() { Options = dict };
        if (!string.IsNullOrWhiteSpace(id))
        {
            if (!Guid.TryParse(id, out Guid guid)) { throw new ArgumentException(new StringBuilder().Append(id).Append(" is not valid tenant id").ToString()); }
            var repo = _serviceProvider.GetRequiredService<Repository<long, TenantDetails>>();
            var tenantDetails = await repo.FindAsync(new QueryRequest().Where(nameof(TenantDetails.TenantId), FieldOperator.Equal, guid), CancellationToken.None);
            var tenant = tenantDetails?.FirstOrDefault();
            if (tenant == null)
            {
                throw new ArgumentException(new StringBuilder().Append(id).Append(" is not valid tenant id").ToString());
            }
            return ToTenant(tenant);
            // var tenant = await GetTenantById(_options.ConnectionString, guid)
            //             ?? throw new ArgumentException(new StringBuilder().Append(id).Append(" is not valid tenant id").ToString());
            // result = ToTenant(tenant);
        }
        return result;
    }
    public async Task<IEnumerable<ITenant>> GetAllTenantsAsync()
    {
        var repo = _serviceProvider.GetRequiredService<Repository<long, TenantDetails>>();
        var result = await repo.FindAsync(new QueryRequest(), CancellationToken.None);
        return result.Select(ToTenant);
        // var result = GetAllTenants(_options.ConnectionString);
        // var val = await result.ToListAsync();
        // return val.Select(ToTenant);
    }
    private static Tenant ToTenant(TenantDetails tenant)
    {
        var dict = new Dictionary<string, string?>();
        tenant.Options.ForEach(kv => dict[kv.Key] = kv.Value);
        return new Tenant() { Id = tenant.Id.ToString(), Name = tenant.Name, Options = dict };
    }
    // private static async Task<TenantDetails?> GetTenantById(string connectionString, Guid tenantId)
    // {
    //     return await GetAllTenants(connectionString).FirstOrDefaultAsync(x => x.TenantId == tenantId);
    // }

    // private static IQueryable<TenantDetails> GetAllTenants(string connectionString)
    // {
    //     var dbContextOptions = new DbContextOptionsBuilder().UseNpgsql(connectionString).Options;
    //     using var dbcontext = new DbContext(dbContextOptions);
    //     return dbcontext.Set<TenantDetails>().AsQueryable().AsNoTracking();
    // }
}
