namespace SparkPlug.Business.RBAC.Domain;

[TenantDb]
public class Resource : IBaseEntity<long>, IConcurrencyEntity, IAuditableEntity<long>, IDeletableEntity
{
    public long Id { get; set; } = default;
    public string Name { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    [ConcurrencyCheck]
    public int Revision { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}
