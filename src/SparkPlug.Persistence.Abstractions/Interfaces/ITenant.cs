namespace SparkPlug.Persistence.Abstractions;

public interface ITenant
{
    public string? Id { get; }
    public string? Name { get; }
    public IDictionary<string, string?> Options { get; }
}
