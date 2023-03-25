namespace SparkPlug.Contracts;

public enum FieldOperator
{
    [EnumMember(Value = "eq")] Equal,
    [EnumMember(Value = "ne")] NotEqual,
    [EnumMember(Value = "gt")] GreaterThan,
    [EnumMember(Value = "ge")] GreaterThanOrEqual,
    [EnumMember(Value = "lt")] LessThan,
    [EnumMember(Value = "le")] LessThanOrEqual,
    [EnumMember] In,
    [EnumMember] NotIn,
    [EnumMember] Contains,
    [EnumMember(Value = "starts")] StartsWith,
    [EnumMember(Value = "ends")] EndsWith,
    [EnumMember(Value = "bw")] Between,
}

public class FieldFilter : ConditionFilter, IFieldFilter
{
    public FieldFilter(string field, FieldOperator op, object value) : base(field, FilterType.Field)
    {
        Op = op;
        Value = value;
    }
    public FieldOperator Op { get; set; }
    public object? Value { get; set; }
    public FilterValueType? Type { get; set; }
}
