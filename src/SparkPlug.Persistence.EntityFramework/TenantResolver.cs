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
            if (!Guid.TryParse(id, out Guid gid)) { throw new ArgumentException($"{id} is not valid tenant id"); }
            var repo = _serviceProvider.GetRequiredService<Repository<Guid, TenantDetails>>();
            var tenantDetails = await repo.GetAsync(gid, CancellationToken.None);
            tenantDetails.Options.ForEach(x => dict[x.Key] = x.Value);
            result = new Tenant() { Id = tenantDetails.Id.ToString(), Name = tenantDetails.Name, Options = dict };
        }
        return result;
    }
}
