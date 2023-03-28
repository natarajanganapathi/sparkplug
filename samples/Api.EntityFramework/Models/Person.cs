namespace SparkPlug.Sample.WebApi.Models;

[Api("person", typeof(ApiController<,>))]
public class Person : BaseEntity<long>, IConcurrencyEntity, IAuditableEntity<long>, IDeletableEntity
{
    public string? PersonName { get; set; }
    public string? Department { get; set; }
    public long Salary { get; set; }
    public long? Age { get; set; }
    public long AddressId { get; set; }
    public Address? Address { get; set; }
    public string? MobileNo { get; set; }
    [ConcurrencyCheck]
    public int Revision { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}

public class Address : BaseEntity<long>, IConcurrencyEntity, IAuditableEntity<long>, IDeletableEntity
{
    public string? FlatNo { get; set; }
    public string? Street { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Pin { get; set; }
    [JsonIgnore]
    public Person? Person { get; set; }
    [ConcurrencyCheck]
    public int Revision { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}
