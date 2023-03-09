namespace SparkPlug.Persistence.EntityFramework;

public static class FilterExtentions
{
    public static IEnumerable<Expression<Func<TEntity, bool>>?> GetFilterExpressions<TEntity>(this IFilter[] filters)
    {
        return filters.Select(x => x.GetFilterExpression<TEntity>()).ToArray();
    }
    public static Expression<Func<TEntity, bool>>? GetFilterExpression<TEntity>(this IFilter filter)
    {
        return filter switch
        {
            ICompositeFilter compositeFilter => compositeFilter.GetFilterExpression<TEntity>(),
            IFieldFilter fieldFilter => fieldFilter.GetFilterExpression<TEntity>(),
            IUnaryFilter unaryFilter => unaryFilter.GetFilterExpression<TEntity>(),
            _ => throw new NotSupportedException($"Filter type {filter.GetType().Name} is not supported")
        };
    }
    public static Expression<Func<TEntity, bool>>? GetFilterExpression<TEntity>(this ICompositeFilter compositeFilter)
    {
        return compositeFilter.Op switch
        {
            CompositeOperator.And => compositeFilter.Filters?.GetFilterExpressions<TEntity>()?.MergeExpressions(CompositeOperator.And),
            CompositeOperator.Or => compositeFilter.Filters?.GetFilterExpressions<TEntity>()?.MergeExpressions(CompositeOperator.Or),
            _ => throw new QueryEntityException("Invalid composite filter operation")
        };
    }
    public static Expression<Func<TEntity, bool>>? MergeExpressions<TEntity>(this IEnumerable<Expression<Func<TEntity, bool>>?> expressions, CompositeOperator op)
    {
        Expression<Func<TEntity, bool>>? result = expressions.FirstOrDefault();
        if (result != null)
        {
            foreach (var expression in expressions.Skip(1))
            {
                if (expression != null)
                {
                    var invokedExpr = Expression.Invoke(expression, result.Parameters.Cast<Expression>());
                    result = Expression.Lambda<Func<TEntity, bool>>(CompositeOperator.And == op ? Expression.AndAlso(result.Body, invokedExpr) : Expression.OrElse(result.Body, invokedExpr), result.Parameters);
                }
            }
        }
        return result;
    }
    public static Expression<Func<TEntity, bool>> GetFilterExpression<TEntity>(this IFieldFilter fieldFilter)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "ffilter");
        var left = Expression.Property(parameter, fieldFilter.Field);
        var right = Expression.Constant(fieldFilter.Value);
        Expression body = fieldFilter.Op switch
        {
            FieldOperator.Equal => Expression.Equal(left, right),
            FieldOperator.NotEqual => Expression.NotEqual(left, right),
            FieldOperator.GreaterThan => Expression.GreaterThan(left, right),
            FieldOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, right),
            FieldOperator.LessThan => Expression.LessThan(left, right),
            FieldOperator.LessThanOrEqual => Expression.LessThanOrEqual(left, right),
            FieldOperator.In => Expression.Call(Expression.Constant(fieldFilter.Value), "Contains", null, left),
            FieldOperator.NotIn => Expression.Not(Expression.Call(Expression.Constant(fieldFilter.Value), "Contains", null, left)),
            _ => throw new QueryEntityException("Invalid field filter operation")
        };
        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }
    public static Expression<Func<TEntity, bool>> GetFilterExpression<TEntity>(this IUnaryFilter unaryFilter)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "ufilter");
        var left = Expression.Property(parameter, unaryFilter.Field);
        var right = Expression.Constant(unaryFilter.Op == UnaryOperator.IsNull ? null : DBNull.Value);
        var body = unaryFilter.Op == UnaryOperator.IsNull ? Expression.Equal(left, right) : Expression.NotEqual(left, right);
        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }
}
