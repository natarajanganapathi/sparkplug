namespace SparkPlug.Persistence.Abstractions;

public enum Status
{
    [EnumMember] Active = 1,
    [EnumMember] Deleted = 2
}
public interface IDeletableEntity
{
    Status Status { get; set; }
}
