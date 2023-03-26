namespace SparkPlug.Persistence.Abstractions;

public class Tenant : ITenant
{
    public static ITenant Default { get => new Tenant() { Id = string.Empty, Name = string.Empty }; }
    public string? Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<KeyValuePair<string, string?>> Options { get; set; } = new List<KeyValuePair<string, string?>>();
}