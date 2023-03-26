namespace SparkPlug.Sample.WebApi.Models;

[Collection("Persons")]
[Api("person", typeof(ApiController<,>))]
public class Person : BaseEntity<string>
{
    public string? PersonName { get; set; }
    public string? Department { get; set; }
    public long Salary { get; set; }
    public Address? Address { get; set; }
    public string? MobileNo { get; set; }
}

public class Address
{
    public string? FlatNo { get; set; }
    public string? Street { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Pin { get; set; }
}
