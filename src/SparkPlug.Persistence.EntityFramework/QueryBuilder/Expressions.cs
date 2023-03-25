namespace SparkPlug.Persistence.EntityFramework;

public static class Expressions
{
    public static Expression Parse(object? value, Type type)
    {
        var parseMethod = type.GetMethod("Parse", new[] { Types.StringType }) ?? throw new Exception($"type {type} does not have Parse method");
        return Expression.Call(parseMethod, Expression.Constant(value));
    }
    public static Expression ToUniversalTime(object value)
    {
        return Expression.Call(Parse(value, Types.DateTimeType), "ToUniversalTime", null);
    }
}