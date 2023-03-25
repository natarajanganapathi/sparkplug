namespace SparkPlug.Persistence.EntityFramework;

public static class FilterExtentions
{
    public static Expression<Func<TEntity, bool>>? GetFilterExpression<TEntity>(this IFilter filter)
    {
        return filter switch
        {
            ICompositeFilter compositeFilter => compositeFilter.GetFilterExpression<TEntity>(),
            IFieldFilter fieldFilter => fieldFilter.GetFilterExpression<TEntity>(),
            IUnaryFilter unaryFilter => unaryFilter.GetFilterExpression<TEntity>(),
            _ => throw new QueryEntityException(new StringBuilder().Append("Filter type ").Append(filter.GetType().Name).Append("is not supported").ToString())
        };
    }

    #region Composite Filter
    private static Expression<Func<TEntity, bool>>? GetFilterExpression<TEntity>(this ICompositeFilter compositeFilter)
    {
        return compositeFilter.Op switch
        {
            CompositeOperator.And => compositeFilter.Filters?.GetFilterExpressions<TEntity>()?.MergeExpressions(CompositeOperator.And),
            CompositeOperator.Or => compositeFilter.Filters?.GetFilterExpressions<TEntity>()?.MergeExpressions(CompositeOperator.Or),
            _ => throw new QueryEntityException(new StringBuilder().Append("Invalid composite filter operation - ").Append(compositeFilter.Op).ToString())
        };
    }
    private static IEnumerable<Expression<Func<TEntity, bool>>?> GetFilterExpressions<TEntity>(this IFilter[] filters)
    {
        return filters.Select(x => x.GetFilterExpression<TEntity>());
    }
    private static Expression<Func<TEntity, bool>>? MergeExpressions<TEntity>(this IEnumerable<Expression<Func<TEntity, bool>>?> expressions, CompositeOperator op)
    {
        Expression<Func<TEntity, bool>>? result = expressions.FirstOrDefault();
        if (result != null)
        {
            foreach (var expression in expressions.Skip(1))
            {
                if (expression != null)
                {
                    var invokedExpr = Expression.Invoke(expression, result.Parameters.Cast<Expression>());
                    result = Expression.Lambda<Func<TEntity, bool>>(CompositeOperator.And == op
                                ? Expression.AndAlso(result.Body, invokedExpr)
                                : Expression.OrElse(result.Body, invokedExpr),
                            result.Parameters);
                }
            }
        }
        return result;
    }
    #endregion

    #region Field Filter
    private static readonly MethodInfo ContainsMethod = Types.EnumerableType.GetMethods()
                        .Single(m => m.Name == Names.EnumerableContains && m.GetParameters().Length == 2);

    private static Expression<Func<TEntity, bool>> GetFilterExpression<TEntity>(this IFieldFilter fieldFilter)
    {
        var tEntityType = typeof(TEntity);
        var parameter = Expression.Parameter(tEntityType, tEntityType.Name);
        var left = PropertyExpression(parameter, fieldFilter);
        Expression body = fieldFilter.Op switch
        {
            FieldOperator.Equal => Expression.Equal(left, ConstantValueExpression(fieldFilter.Value, left.Type, fieldFilter.Type)),
            FieldOperator.NotEqual => Expression.NotEqual(left, ConstantValueExpression(fieldFilter.Value, left.Type, fieldFilter.Type)),
            FieldOperator.GreaterThan => Expression.GreaterThan(left, ConstantValueExpression(fieldFilter.Value, left.Type, fieldFilter.Type)),
            FieldOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, ConstantValueExpression(fieldFilter.Value, left.Type, fieldFilter.Type)),
            FieldOperator.LessThan => Expression.LessThan(left, ConstantValueExpression(fieldFilter.Value, left.Type, fieldFilter.Type)),
            FieldOperator.LessThanOrEqual => Expression.LessThanOrEqual(left, ConstantValueExpression(fieldFilter.Value, left.Type, fieldFilter.Type)),
            FieldOperator.Contains => Expression.Call(left, "Contains", null, ConstantValueExpression(fieldFilter.Value, left.Type, fieldFilter.Type)),
            FieldOperator.StartsWith => Expression.Call(left, "StartsWith", null, ConstantValueExpression(fieldFilter.Value, left.Type, fieldFilter.Type)),
            FieldOperator.EndsWith => Expression.Call(left, "EndsWith", null, ConstantValueExpression(fieldFilter.Value, left.Type, fieldFilter.Type)),
            FieldOperator.In => InOperatorExpresson(left, fieldFilter),
            FieldOperator.NotIn => Expression.Not(InOperatorExpresson(left, fieldFilter)),
            FieldOperator.Between => BetweenOperatorExpression(left, fieldFilter),
            _ => throw new QueryEntityException(new StringBuilder().Append("Invalid field filter operation - ").Append(fieldFilter.Op).ToString())
        };
        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }
    private static Expression PropertyExpression(ParameterExpression parameter, IFieldFilter filter)
    {
        var path = filter.Field.Split(".");
        var property = Expression.Property(parameter, path[0]);
        if (path.Length > 1)
        {
            (property, _) = GetParameterExpression(property, path[1..]);
        }
        // return property.Type switch
        // {
        //     _ => property
        // };
        return property;
    }
    private static (MemberExpression property, string[] path) GetParameterExpression(MemberExpression property, string[] path)
    {
        return path.Length > 0 ? GetParameterExpression(Expression.Property(property, path[0]), path[1..]) : (property, path);
    }
    private static Expression ConstantValueExpression(object? value, Type type, FilterValueType? valueType)
    {
        if (value == null) throw new Exception(new StringBuilder().Append("value cannot be empty").ToString());
        return type switch
        {
            { FullName: "System.DateTime" } => ToDateTimeExpression(value, valueType),
            { FullName: "System.Int64" } or { FullName: "System.Int32" } => Expressions.Parse(value, type),
            { IsEnum: true } => Expression.Convert(Expression.Constant(value), type),
            _ => Expression.Constant(value)
        };
    }
    private static Expression ToDateTimeExpression(object value, FilterValueType? valueType)
    {
        valueType ??= FilterValueType.UtcDateTime;
        return valueType switch
        {
            FilterValueType.UtcDateTime => Expressions.ToUniversalTime(value),
            FilterValueType.DateOnly => StringToDateOnlyExpression(value),
            _ => throw new QueryEntityException(new StringBuilder().Append(valueType).Append(" is not valid type for FilterValueType").ToString())
        };
    }
    private static Expression StringToDateOnlyExpression(object value)
    {
        throw new QueryEntityException("StringToDateOnlyExpression is Not Implemented. Please override this method");
    }
    private static Expression InOperatorExpresson(Expression left, IFieldFilter filter)
    {
        var right = filter.Value;
        if (right == null || right is not JArray) throw new ArgumentException("Field Filter value should not be null", nameof(filter));
        return Expression.Call(ContainsMethod.MakeGenericMethod(left.Type), Expression.Constant((right as JArray)?.ToObject(left.Type.MakeArrayType())), left);
    }
    private static Expression BetweenOperatorExpression(Expression left, IFieldFilter filter)
    {
        var right = filter.Value;
        if (right == null || right is not JArray) throw new ArgumentException("Field Filter value cannot be null", nameof(filter));
        var values = (right as JArray)?.ToObject<string[]>();
        if (values == null || values.Length != 2) throw new QueryEntityException("Field filter values cannot be null and should contain a maximum of 2 values");
        var startValue = ConstantValueExpression(values[0], left.Type, filter.Type);
        var endValue = ConstantValueExpression(values[1], left.Type, filter.Type);
        return Expression.AndAlso(Expression.GreaterThanOrEqual(left, startValue), Expression.LessThanOrEqual(left, endValue));
    }
    #endregion

    #region Unary Filter
    private static Expression<Func<TEntity, bool>> GetFilterExpression<TEntity>(this IUnaryFilter unaryFilter)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "ufilter");
        var left = Expression.Property(parameter, unaryFilter.Field);
        var right = Expression.Constant(unaryFilter.Op == UnaryOperator.IsNull ? null : DBNull.Value);
        Expression body = unaryFilter.Op switch
        {
            UnaryOperator.IsNull => Expression.Equal(left, right),
            UnaryOperator.IsNotNull => Expression.NotEqual(left, right),
            _ => throw new QueryEntityException($"Invalid unary filter operation - {unaryFilter.Op}")
        };
        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }
    #endregion
}
