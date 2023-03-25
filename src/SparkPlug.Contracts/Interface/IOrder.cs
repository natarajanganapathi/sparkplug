namespace SparkPlug.Contracts;

public enum Direction
{
    [EnumMember(Value = "ASC")] Ascending,
    [EnumMember(Value = "DESC")] Descending
}

public interface IOrder
{
    string Field { get; set; }
    Direction Direction { get; set; }
}
