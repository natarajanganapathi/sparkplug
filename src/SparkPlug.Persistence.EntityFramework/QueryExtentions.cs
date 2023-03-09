namespace SparkPlug.Persistence.EntityFramework;

public static class QueryExtentions
{
    public static IQueryable<TEntity> ApplyWhere<TEntity>(this IQueryable<TEntity> query, Filter? filter)
    {
        if (filter == null) return query;

        var where = filter.GetFilterExpression<TEntity>();
        return where == null ? query : query.Where(where);
    }
    public static IQueryable<TEntity> ApplySelector<TEntity>(this IQueryable<TEntity> query, string[]? select)
    {
        if (select == null || select.Length == 0) return query;

        var TEntityType = typeof(TEntity);
        var properties = TEntityType.GetProperties().Where(prop => select.Contains(prop.Name));
        var parameter = Expression.Parameter(TEntityType, "select");
        var propertyBindings = properties.Select(property => Expression.Bind(property, Expression.MakeMemberAccess(parameter, property)));
        var selector = Expression.Lambda<Func<TEntity, TEntity>>(Expression.MemberInit(Expression.New(TEntityType), propertyBindings), parameter);
        return query.Select(selector);
    }

    public static IQueryable<TEntity> ApplySort<TEntity>(this IQueryable<TEntity> query, Order[]? sort)
    {
        if (sort == null || sort.Length == 0) return query;

        var orderedQuery = query.OrderBy(_ => 0);
        foreach (var order in sort)
        {
            var parameterExp = Expression.Parameter(typeof(TEntity), "sort");
            var propertyExp = Expression.Property(parameterExp, order.Field);
            var lambdaExp = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(propertyExp, typeof(object)), parameterExp);
            orderedQuery = order.Direction == Direction.Ascending ? orderedQuery.ThenBy(lambdaExp) : orderedQuery.ThenByDescending(lambdaExp);
        }
        return orderedQuery;
    }

    public static IQueryable<TEntity> ApplyPageContext<TEntity>(this IQueryable<TEntity> query, PageContext? pageContext)
    {
        if (pageContext == null) return query;
        return query.Skip(pageContext.Skip).Take(pageContext.PageSize);
    }
}