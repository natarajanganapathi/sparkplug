namespace SparkPlug.Persistence.Abstractions;

public interface IRepository<TId, TEntity> where TEntity : class, IBaseEntity<TId>
{
    Task<IList<TEntity>> FindAsync(IQueryRequest request, CancellationToken cancellationToken = default);
    Task<IList<JObject>> QueryAsync(IQueryRequest request, CancellationToken cancellationToken = default);
    Task<long> CountAsync(IQueryRequest request, CancellationToken cancellationToken = default);
    Task<TEntity> GetAsync(TId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetManyAsync(TId[] ids, CancellationToken cancellationToken = default);
    Task<TEntity> CreateAsync(ICommandRequest<TEntity> request, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> CreateManyAsync(ICommandRequest<TEntity[]> request, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TId id, ICommandRequest<TEntity> request, CancellationToken cancellationToken = default);
    Task<TEntity> PatchAsync(TId id, ICommandRequest<JsonPatchDocument<TEntity>> request, CancellationToken cancellationToken = default);
    Task<TEntity> ReplaceAsync(TId id, ICommandRequest<TEntity> request, CancellationToken cancellationToken = default);
    Task<TEntity> DeleteAsync(TId id, CancellationToken cancellationToken = default);
}
