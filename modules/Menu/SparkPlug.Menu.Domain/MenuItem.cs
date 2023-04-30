namespace SparkPlug.Menu.Domain;

[TenantDb]
public class MenuItem : IBaseEntity<long>, IConcurrencyEntity, IAuditableEntity<long>, IDeletableEntity
{
    public long Id { get; set; }
    public long? ParentMenuId { get; set; }
    public long? ModuleId { get; set; }
    public String? ModuleCode { get; set; }
    public String? MenuCode { get; set; }
    public String? ParentMenuCode { get; set; }
    public String? MenuType { get; set; }
    public String? MenuPosition { get; set; }
    public String? Display { get; set; }
    public String? SRef { get; set; }
    public String? IconRef { get; set; }
    public int DisplayOrder { get; set; }
    [ConcurrencyCheck]
    public int Revision { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}