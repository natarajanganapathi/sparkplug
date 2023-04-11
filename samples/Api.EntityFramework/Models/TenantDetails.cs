namespace SparkPlug.Sample.WebApi.Models;

[Api("tenants"), HomeDbEntity]
public class TenantDetails : BaseEntity<long>, IConcurrencyEntity, IAuditableEntity<long>, IDeletableEntity
{
    public Guid TenantId { get; set; }
    public string? Name { get; set; }
    public List<Options> Options { get; set; } = new List<Options>();
    [ConcurrencyCheck]
    public int Revision { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}

public class Options : BaseEntity<long>, IConcurrencyEntity, IAuditableEntity<long>, IDeletableEntity
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public long TenantDetailId { get; set; }
    [JsonIgnore]
    public TenantDetails TenantDetails { get; set; } = new();
    [ConcurrencyCheck]
    public int Revision { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}