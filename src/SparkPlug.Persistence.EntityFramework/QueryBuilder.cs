namespace SparkPlug.Persistence.EntityFramework;

public class QueryBuilder<TEntity> where TEntity : class
{
    protected IQueryRequest? Request { get; }
    protected IQueryable<TEntity> Query { get; set; }
    public QueryBuilder(DbSet<TEntity> dbSet, IQueryRequest? request)
    {
        Request = request;
        Query = dbSet.AsQueryable().AsNoTracking();
    }
    public IQueryable<TEntity> Build()
    {
        return Request == null ? Query : Query.ApplySelector(Request.Select)
                                              .ApplyWhere(Request.Where)
                                              .ApplySort(Request.Sort)
                                              .ApplyPageContext(Request.Page);
    }
}