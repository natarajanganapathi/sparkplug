namespace SparkPlug.Sample.WebApi.Repositories;

public class TenantResolver : ITenantResolver
{
    private readonly IServiceProvider _serviceProvider;
    public TenantResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ITenant> ResolveAsync(string? id)
    {
        return string.IsNullOrWhiteSpace(id) ? GetDefaultTenant() : await GetTenant(id);
    }

    public async Task<Tenant> GetTenant(string id)
    {
        var dict = new Dictionary<string, string?>();
        var repo = _serviceProvider.GetRequiredService<Repository<string, TenantDetails>>();
        var tenantDetails = await repo.GetAsync(id, CancellationToken.None);
        tenantDetails.Options.ForEach(x => dict[x.Key] = x.Value);
        return new Tenant() { Id = tenantDetails.Id, Name = tenantDetails.Name, Options = dict };
    }

    public Tenant GetDefaultTenant()
    {
        var options = _serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>().Value;
        var dict = new Dictionary<string, string?>()
        {
            { $"{nameof(TenantConfig)}:{nameof(options.ConnectionString)}", options.ConnectionString }
        };
        return new Tenant() { Options = dict };
    }
}
