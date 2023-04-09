namespace SparkPlug.Persistence.EntityFramework;

public class TenantRepository<TId, TEntity> : SqlRepository<TId, TEntity> where TEntity : class, IBaseEntity<TId>, new()
{
    public TenantRepository(IServiceProvider serviceProvider, TenantDbContext dbContext) : base(serviceProvider, dbContext)
    {
    }
}