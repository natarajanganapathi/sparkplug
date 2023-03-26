namespace SparkPlug.Contracts;

public enum FieldOperator
{
    [EnumMember(Value = "eq")] Equal,
    [EnumMember(Value = "ne")] NotEqual,
    [EnumMember(Value = "gt")] GreaterThan,
    [EnumMember(Value = "ge")] GreaterThanOrEqual,
    [EnumMember(Value = "lt")] LessThan,
    [EnumMember(Value = "le")] LessThanOrEqual,
    [EnumMember(Value = "bw")] Between,
    [EnumMember(Value = "in")] In,
    [EnumMember(Value = "notin")] NotIn,
    [EnumMember(Value = "ends")] EndsWith,
    [EnumMember(Value = "starts")] StartsWith,
    [EnumMember(Value = "contains")] Contains,
}

public class FieldFilter : ConditionFilter, IFieldFilter
{
    public FieldFilter(string field, FieldOperator op, object value, FilterValueType? type = default) : base(field, FilterType.Field)
    {
        Op = op;
        Value = value;
        Type = type;
    }
    public FieldOperator Op { get; set; }
    public object? Value { get; set; }
    public FilterValueType? Type { get; set; }
}
