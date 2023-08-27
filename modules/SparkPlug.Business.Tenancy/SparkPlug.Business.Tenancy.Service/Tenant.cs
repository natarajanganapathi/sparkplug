namespace SparkPlug.Business.Tenancy.Service;

public interface ITenant
{
    public string? Id { get; }
    public string? Name { get; }
    public IDictionary<string, string?> Options { get; }
}

public interface ITenantResolver
{
    Task<ITenant> ResolveAsync(string? id);
    Task<IEnumerable<ITenant>> GetAllTenantsAsync();
}

public class Tenant : ITenant
{
    public static ITenant Default { get => new Tenant() { Id = string.Empty, Name = string.Empty }; }
    public string? Id { get; set; }
    public string? Name { get; set; }
    public IDictionary<string, string?> Options { get; set; } = new Dictionary<string, string?>();
}