namespace SparkPlug.Persistence.EntityFramework;

public class QueryBuilder<TEntity> where TEntity : class
{
    protected IQueryRequest? Request { get; }
    protected IQueryable<TEntity> Query { get; }
    public QueryBuilder(DbSet<TEntity> dbSet, IQueryRequest? request)
    {
        Request = request;
        Query = dbSet.AsQueryable().AsNoTracking();
    }
    private IQueryable<TEntity> Get()
    {
        return Request == null ? Query
                               : Query.ApplyWhere(Request.Where)
                                 .ApplySort(Request.Sort)
                                 .ApplyPageContext(Request.Page)
                                 .ApplyIncludes(Request.Includes);
    }
    public IQueryable<JObject> Project() => Get().ApplyProjection(Request?.Select, Request?.Includes);
    public IQueryable<TEntity> Select() => Get().ApplySelector(Request?.Select, Request?.Includes);

    public async Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        var query = Request == null ? Query
                                    : Query.ApplyWhere(Request.Where);
        return await query.LongCountAsync(cancellationToken);
    }
}