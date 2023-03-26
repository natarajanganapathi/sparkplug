namespace SparkPlug.Persistence.EntityFramework.Models;

public class TenantDetails : BaseEntity<Guid>, IConcurrencyEntity, IAuditableEntity<Guid>, IDeletableEntity
{
    public string? Name { get; set; }
    public List<Options> Options { get; set; } = new List<Options>();
    [ConcurrencyCheck]
    public int Revision { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public Guid ModifiedBy { get; set; }
}

public class Options : BaseEntity<Guid>, IConcurrencyEntity, IAuditableEntity<Guid>, IDeletableEntity
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    [ConcurrencyCheck]
    public int Revision { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public Guid ModifiedBy { get; set; }
}