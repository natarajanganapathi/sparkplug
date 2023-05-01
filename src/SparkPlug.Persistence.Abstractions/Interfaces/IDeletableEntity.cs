namespace SparkPlug.Persistence.Abstractions;

public enum Status
{
    [EnumMember] Live = 1,
    [EnumMember] Deleted = 2
}
public interface IDeletableEntity
{
    Status Status { get; set; }
}
