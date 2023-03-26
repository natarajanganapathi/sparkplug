namespace SparkPlug.Persistence.Abstractions;

public interface IRepositoryProvider
{
    IRepository<TId, TEntity> GetRepository<TId, TEntity>() where TEntity : class, IBaseEntity<TId>, new();
}