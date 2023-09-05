namespace SparkPlug.Persistence.Abstractions;

public class SingleTenant : ITenant
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public IDictionary<string, string?> Options { get; set; } = new Dictionary<string, string?>();
}