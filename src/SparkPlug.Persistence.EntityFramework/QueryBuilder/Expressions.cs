namespace SparkPlug.Persistence.EntityFramework;

public static class Expressions
{
    public static Expression Parse(object? value, Type type)
    {
        var parseMethod = type.GetMethod("Parse", new[] { Types.StringType })
            ?? throw new Exception(new StringBuilder().Append("type ").Append(type).Append("does not have Parse method").ToString());
        return Expression.Call(parseMethod, Expression.Constant(value));
    }
    public static Expression ToDateOnly(object value)
    {
        return Parse(value, typeof(DateOnly));
    }
    public static Expression ToUniversalTime(object value)
    {
        return Expression.Call(Parse(value, Types.DateTimeType), "ToUniversalTime", null);
    }
}