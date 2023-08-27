namespace SparkPlug.Persistence.Abstractions;

public interface IMultiTenantEntity<TId>
{
    TId TenantId { get; set; }
}
