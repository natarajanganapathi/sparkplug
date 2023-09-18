namespace SparkPlug.Persistence.MongoDb;

public class MongoRepositoryProvider : IRepositoryProvider
{
    private readonly IServiceProvider _serviceProvider;
    public MongoRepositoryProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IRepository<TId, TEntity> GetRepository<TId, TEntity>() where TEntity : class, IBaseEntity<TId>, new()
    {
        return _serviceProvider.GetRequiredService<MongoRepository<TId, TEntity>>();
    }
}
