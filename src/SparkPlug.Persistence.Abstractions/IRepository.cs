namespace SparkPlug.Persistence.Abstractions;

public interface IRepository<TId, TEntity> where TEntity : class, IBaseEntity<TId>
{
    Task<IEnumerable<TEntity>> ListAsync(IQueryRequest? request, CancellationToken cancellationToken);
    Task<long> GetCountAsync(IQueryRequest? request, CancellationToken cancellationToken);
    Task<(IEnumerable<TEntity>, long)> ListWithCountAsync(IQueryRequest? request, CancellationToken cancellationToken);
    Task<TEntity> GetAsync(TId id, CancellationToken cancellationToken);
    Task<TEntity[]> GetManyAsync(TId[] ids, CancellationToken cancellationToken);
    Task<TEntity> CreateAsync(ICommandRequest<TEntity> request, CancellationToken cancellationToken);
    Task<TEntity[]> CreateManyAsync(ICommandRequest<TEntity[]> request, CancellationToken cancellationToken);
    Task<TEntity> UpdateAsync(TId id, ICommandRequest<TEntity> request, CancellationToken cancellationToken);
    Task<TEntity> PatchAsync(TId id, ICommandRequest<JsonPatchDocument<TEntity>> request, CancellationToken cancellationToken);
    Task<TEntity> ReplaceAsync(TId id, ICommandRequest<TEntity> request, CancellationToken cancellationToken);
    Task<TEntity> DeleteAsync(TId id, CancellationToken cancellationToken);
}