namespace SparkPlug.Business.Menu.Domain;

[TenantDb]
public class MenuItem : IBaseEntity<long>, IConcurrencyEntity, IAuditableEntity<long>, IDeletableEntity
{
    public long Id { get; set; }
    public long? ParentMenuId { get; set; }
    public long? ModuleId { get; set; }
    public string? ModuleCode { get; set; }
    public string? MenuCode { get; set; }
    public string? ParentMenuCode { get; set; }
    public string? MenuType { get; set; }
    public string? MenuPosition { get; set; }
    public string? Label { get; set; }
    public string? SRef { get; set; }
    public string? IconRef { get; set; }
    public int DisplayOrder { get; set; }
    [ConcurrencyCheck]
    public int Revision { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}