namespace SparkPlug.Persistence.Abstractions;

public interface ITenant
{
    public string? Id { get; }
    public string? Name { get; }
    public IEnumerable<KeyValuePair<string, string?>> Options { get; }
}