namespace SparkPlug.Persistence.Abstractions;

public interface IAuditableEntity<TId>
{
    DateTime CreatedAt { get; set; }
    TId CreatedBy { get; set; }
    DateTime ModifiedAt { get; set; }
    TId ModifiedBy { get; set; }
}
