namespace SparkPlug.Persistence.EntityFramework;

public class HomeRepository<TId, TEntity> : SqlRepository<TId, TEntity> where TEntity : class, IBaseEntity<TId>, new()
{
    public HomeRepository(IServiceProvider serviceProvider, HomeDbContext dbContext) : base(serviceProvider, dbContext)
    {
    }
}