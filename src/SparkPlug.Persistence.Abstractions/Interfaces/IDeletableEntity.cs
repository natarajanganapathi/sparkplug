namespace SparkPlug.Persistence.Abstractions;

// [JsonConverter(typeof(StringEnumConverter))]
public enum Status
{
    [EnumMember] Live = 1,
    [EnumMember] Deleted = 2
}
public interface IDeletableEntity
{
    Status Status { get; set; }
}
