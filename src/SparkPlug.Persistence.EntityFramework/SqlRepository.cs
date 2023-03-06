namespace SparkPlug.Persistence.EntityFramework;

public class SqlRepository<TId, TEntity> : IRepository<TId, TEntity> where TEntity : class, IBaseEntity<TId>, new()
{
    public SqlDbContext DbContext { get; }
    internal readonly ILogger<SqlRepository<TId, TEntity>> logger;
    internal readonly IRequestContext<TId> requestContext;
    private DbSet<TEntity>? _dbSet;
    public virtual DbSet<TEntity> DbSet
    {
        get
        {
            return _dbSet ??= GetDbSet();
        }
    }
    public DbSet<TEntity> GetDbSet()
    {
        return DbContext.Set<TEntity>();
    }
    public DbSet<Entity> GetDbSet<Entity>() where Entity : class, IBaseEntity<TId>, new()
    {
        return DbContext.Set<Entity>();
    }
    public SqlRepository(IServiceProvider serviceProvider)
    {
        DbContext = serviceProvider.GetRequiredService<SqlDbContext>();
        logger = serviceProvider.GetRequiredService<ILogger<SqlRepository<TId, TEntity>>>();
        requestContext = serviceProvider.GetRequiredService<IRequestContext<TId>>();
    }
    public async Task<(IEnumerable<TEntity>, long)> ListWithCountAsync(IQueryRequest? request, CancellationToken cancellationToken)
    {
        var query = GetQuery(request);
        logger.LogInformation("Search Query: {query}", query.ToQueryString());
        return (await query.ToListAsync(cancellationToken), await query.LongCountAsync(cancellationToken));
    }
    public async Task<IEnumerable<TEntity>> ListAsync(IQueryRequest? request, CancellationToken cancellationToken)
    {
        return await GetQuery(request).ToListAsync(cancellationToken);
    }
    public IQueryable<TEntity> GetQuery(IQueryRequest? request)
    {
        var query = GetDbSet().AsQueryable().AsNoTracking();
        if (request != null)
        {
            query = request.Select?.Any() == true ? request.Select.GetSelector(query) : query;
            query = request.Sort?.Any() == true ? request.Sort.GetSort(query) : query;
            query = request.Where != null ? request.Where.GetWhere(query) : query;
            query = request.Page != null ? request.Page.GetPageContext(query) : query;
        }
        return query;
    }
    public async Task<TEntity> GetAsync(TId id, CancellationToken cancellationToken)
    {
        var tid = id ?? throw new QueryEntityException("Id is null");
        var result = await DbSet.FindAsync(new object[] { tid }, cancellationToken).ConfigureAwait(false);
        return result ?? throw new QueryEntityException("Id is not found");
    }
    public async Task<TEntity[]> GetManyAsync(TId[] ids, CancellationToken cancellationToken)
    {
        var tids = ids ?? throw new QueryEntityException("Ids are null or empty");
        return await DbSet.Where(x => ids.Contains(x.Id)).ToArrayAsync(cancellationToken);
    }
    public async Task<TEntity> CreateAsync(ICommandRequest<TEntity> request, CancellationToken cancellationToken)
    {
        var entity = request.Data ?? throw new CreateEntityException("Entity is null");
        var entityEntry = await DbSet.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(requestContext.UserId, cancellationToken);
        return entityEntry.Entity;
    }
    public async Task<TEntity[]> CreateManyAsync(ICommandRequest<TEntity[]> requests, CancellationToken cancellationToken)
    {
        var entities = requests.Data ?? throw new CreateEntityException("Entities are null");
        await DbSet.AddRangeAsync(entities, cancellationToken);
        await DbContext.SaveChangesAsync(requestContext.UserId, cancellationToken);
        return entities;
    }
    public async Task<TEntity> UpdateAsync(TId id, ICommandRequest<TEntity> request, CancellationToken cancellationToken)
    {
        var entity = request.Data ?? throw new UpdateEntityException("Entity is null");
        entity.Id = id ?? throw new UpdateEntityException("Id is null");
        return await UpdateAsync(entity, cancellationToken);
    }
    private async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        DbSet.Attach(entity);
        if (entity is IConcurrencyEntity obj) { obj.Revision++; }
        DbContext.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync(requestContext.UserId, cancellationToken);
        return entity;
    }
    public async Task<TEntity> DeleteAsync(TId id, CancellationToken cancellationToken)
    {
        var tid = id ?? throw new DeleteEntityException("Id is null");
        TEntity entityToDelete = (await DbSet.FindAsync(new object[] { tid }, cancellationToken).ConfigureAwait(false)) ?? throw new DeleteEntityException("Id is invalid");
        if (entityToDelete is IDeletableEntity obj) { obj.Status = Status.Deleted; }
        return await UpdateAsync(entityToDelete, cancellationToken);
    }
    public async Task<long> GetCountAsync(IQueryRequest? request, CancellationToken cancellationToken)
    {
        return await GetQuery(request).LongCountAsync(cancellationToken).ConfigureAwait(false);
    }
    public async Task<TEntity> PatchAsync(TId id, ICommandRequest<JsonPatchDocument<TEntity>> request, CancellationToken cancellationToken)
    {
        var patchDocument = request.Data ?? throw new UpdateEntityException("Entity is null");
        var tid = id ?? throw new UpdateEntityException("Id is null");
        TEntity original = await GetAsync(tid, cancellationToken);
        // patchDocument.Operations.Add(new Operation<TEntity>("replace", $"/{nameof(IAuditableEntity.ModifiedBy)}", null, _user.UserId));
        // patchDocument.Operations.Add(new Operation<TEntity>("replace", $"/{nameof(IAuditableEntity.ModifiedAt)}", null, DateTime.UtcNow));
        patchDocument.ApplyTo(original);
        return await UpdateAsync(original, cancellationToken);
    }
    public async Task<TEntity> ReplaceAsync(TId id, ICommandRequest<TEntity> request, CancellationToken cancellationToken)
    {
        var entity = request.Data ?? throw new UpdateEntityException("Entity is null");
        var tid = id ?? throw new UpdateEntityException("Id is null");
        var sourceEntity = await GetAsync(tid, cancellationToken).ConfigureAwait(false);
        sourceEntity = sourceEntity ?? throw new UpdateEntityException("Id is invalid");
        DbContext.Entry(sourceEntity).CurrentValues.SetValues(entity);
        var modifyedCount = await DbContext.SaveChangesAsync(requestContext.UserId, cancellationToken);
        if (modifyedCount == 0) throw new UpdateEntityException("No records are replaced");
        return sourceEntity;
    }
}

public static class QueryExtentions
{
    public static IQueryable<TEntity> ApplyWhere<TEntity>(this IQueryable<TEntity> query, Filter filter)
    {
        var where = filter.GetFilterDefinition<TEntity>();
        return query.Where(where);
    }
    public static IQueryable<TEntity> ApplySelector<TEntity>(this IQueryable<TEntity> query, string[] select)
    {
        if(select == null || select.Length == 0)
            return query;

        var TEntityType = typeof(TEntity);
        var properties = TEntityType.GetProperties().Where(prop => select.Contains(prop.Name));
        var parameter = Expression.Parameter(TEntityType, "select");
        var propertyBindings = properties.Select(property => Expression.Bind(property, Expression.MakeMemberAccess(parameter, property)));
        var selector = Expression.Lambda<Func<TEntity, TEntity>>(Expression.MemberInit(Expression.New(TEntityType), propertyBindings), parameter);
        return query.Select(selector);
    }

    public static IQueryable<TEntity> ApplySort<TEntity>(this IQueryable<TEntity> query, Order[] sort)
    {
        if (sort == null || sort.Length == 0)
            return query;

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

    public static IQueryable<TEntity> ApplyPageContext<TEntity>(this IQueryable<TEntity> query, PageContext pageContext)
    {
        return query.Skip(pageContext.Skip).Take(pageContext.PageSize);
    }

}

public static class Extention
{
    public static IQueryable<TEntity> GetWhere<TEntity>(this Filter filter, IQueryable<TEntity> query)
    {
        var where = filter.GetFilterDefinition<TEntity>();
        return query.Where(where);
    }

    public static IQueryable<TEntity> GetSelector<TEntity>(this string[] select, IQueryable<TEntity> query)
    {
        var TEntityType = typeof(TEntity);
        var properties = TEntityType.GetProperties().Where(prop => select.Contains(prop.Name));
        var parameter = Expression.Parameter(TEntityType, "select");
        var propertyBindings = properties.Select(property => Expression.Bind(property, Expression.MakeMemberAccess(parameter, property)));
        var selector = Expression.Lambda<Func<TEntity, TEntity>>(Expression.MemberInit(Expression.New(TEntityType), propertyBindings), parameter);
        return query.Select(selector);
    }
    public static IQueryable<TEntity> GetSort<TEntity>(this Order[] sort, IQueryable<TEntity> query)
    {
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

    public static IQueryable<TEntity> GetPageContext<TEntity>(this PageContext pageContext, IQueryable<TEntity> query)
    {
        return query.Skip(pageContext.Skip).Take(pageContext.PageSize);
    }

    public static IEnumerable<Expression<Func<TEntity, bool>>> GetFilterDefinitions<TEntity>(this IFilter[] filters)
    {
        return filters.Select(x => x.GetFilterDefinition<TEntity>()).ToArray();
    }

    public static Expression<Func<TEntity, bool>> GetFilterDefinition<TEntity>(this IFilter filter)
    {
        return filter switch
        {
            ICompositeFilter compositeFilter => compositeFilter.GetFilterDefinition<TEntity>(),
            IFieldFilter fieldFilter => fieldFilter.GetFilterDefinition<TEntity>(),
            IUnaryFilter unaryFilter => unaryFilter.GetFilterDefinition<TEntity>(),
            _ => throw new NotSupportedException($"Filter type {filter.GetType().Name} is not supported")
        };
    }

    public static Expression<Func<TEntity, bool>> GetFilterDefinition<TEntity>(this ICompositeFilter compositeFilter)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "cfilter");
        var expression = compositeFilter.Op switch
        {
            CompositeOperator.And => compositeFilter.Filters?.GetFilterDefinitions<TEntity>().MergeExpressions(CompositeOperator.And),
            CompositeOperator.Or => compositeFilter.Filters?.GetFilterDefinitions<TEntity>().MergeExpressions(CompositeOperator.Or),
            _ => throw new QueryEntityException("Invalid composite filter operation")
        };
        Expression<Func<TEntity, bool>> defaultExp = (_) => true;
        return expression ?? defaultExp;
    }
    public static Expression<Func<TEntity, bool>> MergeExpressions<TEntity>(this IEnumerable<Expression<Func<TEntity, bool>>> expressions, CompositeOperator op)
    {
        Expression<Func<TEntity, bool>> result = expressions.First();
        foreach (var expression in expressions.Skip(1))
        {
            var invokedExpr = Expression.Invoke(expression, result.Parameters.Cast<Expression>());
            result = Expression.Lambda<Func<TEntity, bool>>(CompositeOperator.And == op ? Expression.AndAlso(result.Body, invokedExpr) : Expression.OrElse(result.Body, invokedExpr), result.Parameters);
        }
        return result;
    }
    public static Expression<Func<TEntity, bool>> GetFilterDefinition<TEntity>(this IFieldFilter fieldFilter)
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

    public static Expression<Func<TEntity, bool>> GetFilterDefinition<TEntity>(this IUnaryFilter unaryFilter)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "ufilter");
        var left = Expression.Property(parameter, unaryFilter.Field);
        var right = Expression.Constant(unaryFilter.Op == UnaryOperator.IsNull ? null : DBNull.Value);
        var body = unaryFilter.Op == UnaryOperator.IsNull ? Expression.Equal(left, right) : Expression.NotEqual(left, right);
        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }
}
