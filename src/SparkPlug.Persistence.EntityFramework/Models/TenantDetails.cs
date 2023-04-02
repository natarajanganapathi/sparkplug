namespace SparkPlug.Persistence.EntityFramework.Models;

public class TenantDetails : BaseEntity<Guid>, IConcurrencyEntity, IAuditableEntity<long>, IDeletableEntity
{
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
    [ConcurrencyCheck]
    public int Revision { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}