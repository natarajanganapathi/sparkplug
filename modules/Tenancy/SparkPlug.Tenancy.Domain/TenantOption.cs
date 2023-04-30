namespace SparkPlug.Tenancy.Domain;

[HomeDb]
public class TenantOption : IBaseEntity<long>, IConcurrencyEntity, IAuditableEntity<long>, IDeletableEntity
{
    public long Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public long TenantDetailId { get; set; }
    [JsonIgnore]
    public TenantDetails Tenant { get; set; } = new();
    [ConcurrencyCheck]
    public int Revision { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}
