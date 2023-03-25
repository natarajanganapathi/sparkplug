namespace SparkPlug.Contracts;

public enum UnaryOperator
{
    [EnumMember(Value = "nl")] IsNull,
    [EnumMember(Value = "nnl")] IsNotNull
}

public class UnaryFilter : ConditionFilter, IUnaryFilter
{
    public UnaryFilter(string field, UnaryOperator op) : base(field, FilterType.Unary)
    {
        Op = op;
        Field = field;
    }
    public UnaryOperator Op { get; set; }
}
