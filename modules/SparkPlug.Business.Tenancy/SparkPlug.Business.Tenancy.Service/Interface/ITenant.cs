namespace SparkPlug.Business.Tenancy.Service;

public interface ITenant
{
    public string? Id { get; }
    public string? Name { get; }
    public IDictionary<string, string?> Options { get; }
}
