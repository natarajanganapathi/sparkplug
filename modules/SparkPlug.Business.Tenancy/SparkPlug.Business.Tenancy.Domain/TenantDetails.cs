namespace SparkPlug.Business.Tenancy.Domain;

[HomeDb]
public class TenantDetails : IBaseEntity<long>, IConcurrencyEntity, IAuditableEntity<long>, IDeletableEntity
{
    public long Id { get; set; }
    public Guid TenantId { get; set; }
    public string? Name { get; set; }
    public List<TenantOption> Options { get; set; } = new List<TenantOption>();
    [ConcurrencyCheck]
    public int Revision { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}
