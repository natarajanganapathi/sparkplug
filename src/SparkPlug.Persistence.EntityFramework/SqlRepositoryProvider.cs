namespace SparkPlug.Persistence.EntityFramework;

public class SqlRepositoryProvider : IRepositoryProvider
{
    private readonly IServiceProvider _serviceProvider;
    public SqlRepositoryProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IRepository<TId, TEntity> GetRepository<TId, TEntity>() where TEntity : class, IBaseEntity<TId>, new()
    {
        return _serviceProvider.GetRequiredService<SqlRepository<TId, TEntity>>();
    }
}
