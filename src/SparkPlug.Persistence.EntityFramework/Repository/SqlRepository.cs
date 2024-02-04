namespace SparkPlug.Persistence.EntityFramework;

public abstract class SqlRepository<TId, TEntity> : IRepository<TId, TEntity> where TEntity : class, IBaseEntity<TId>, new()
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
    protected SqlRepository(IServiceProvider serviceProvider, SqlDbContext dbContext)
    {
        DbContext = dbContext;
        logger = serviceProvider.GetRequiredService<ILogger<SqlRepository<TId, TEntity>>>();
        requestContext = serviceProvider.GetRequiredService<IRequestContext<TId>>();
    }
    public async Task<IList<JObject>> QueryAsync(IQueryRequest request, CancellationToken cancellationToken = default)
    {
        var query = new QueryBuilder<TEntity>(DbSet, request).Project();
#if DEBUG
        logger.LogInformation("Sql Query: {query}", query.ToQueryString());
#endif
        return await query.ToListAsync(cancellationToken);
    }
    public async Task<IList<TEntity>> FindAsync(IQueryRequest request, CancellationToken cancellationToken = default)
    {
        var query = new QueryBuilder<TEntity>(DbSet, request).Select();
        return await query.ToListAsync(cancellationToken);
    }
    public async Task<long> CountAsync(IQueryRequest request, CancellationToken cancellationToken = default)
    {
        return await new QueryBuilder<TEntity>(DbSet, request).CountAsync(cancellationToken);
    }

    public async Task<TEntity> GetAsync(TId id, CancellationToken cancellationToken = default)
    {
        var tid = id ?? throw new QueryEntityException("Id is null");
        var result = await DbSet.FindAsync([tid], cancellationToken);
        if (result is IDeletableEntity obj)
        {
            result = obj.Status == Status.Live ? result : null;
        }
        return result ?? throw new QueryEntityException($"Id '{id}' is not found");
    }
    public async Task<IEnumerable<TEntity>> GetManyAsync(TId[] ids, CancellationToken cancellationToken = default)
    {
        var results = await DbSet.AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
        var deletedEntities = results.OfType<IDeletableEntity>().Where(x => x.Status != Status.Live);
        if (deletedEntities.Any() || ids.Length != results.Count)
        {
            throw new QueryEntityException("One or more id is not found");
        }
        return results;
    }
    private async Task<Status> GetStats(TId id, CancellationToken cancellationToken = default)
    {
        if (id != null)
        {
            var request = new QueryRequest(new[] { "id", "status" })
                          .Where("Id", FieldOperator.Equal, id);
            var record = (await QueryAsync(request, cancellationToken)).SingleOrDefault();
            if (record != null && record.TryGetValue("status", out var statusValue))
            {
                return (Status)statusValue.Value<int>();
            }
        }
        return Status.Deleted;
    }
    public async Task<TEntity> CreateAsync(ICommandRequest<TEntity> request, CancellationToken cancellationToken = default)
    {
        var entity = request.Data ?? throw new CreateEntityException("Entity is null");
        if (entity is IDeletableEntity obj) { obj.Status = Status.Live; }
        var entityEntry = await DbSet.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(requestContext.UserId, cancellationToken);
        return entityEntry.Entity;
    }
    public async Task<IEnumerable<TEntity>> CreateManyAsync(ICommandRequest<TEntity[]> request, CancellationToken cancellationToken = default)
    {
        var entities = request.Data ?? throw new CreateEntityException("Entities are null");
        foreach (var entity in entities)
        {
            if (entity is IDeletableEntity obj) { obj.Status = Status.Live; }
        }
        await DbSet.AddRangeAsync(entities, cancellationToken);
        await DbContext.SaveChangesAsync(requestContext.UserId, cancellationToken);
        return entities;
    }
    public async Task<TEntity> UpdateAsync(TId id, ICommandRequest<TEntity> request, CancellationToken cancellationToken = default)
    {
        var entity = request.Data ?? throw new UpdateEntityException("Entity is null");
        entity.Id = id ?? throw new UpdateEntityException("Id is null");
        if (entity is IDeletableEntity)
        {
            var status = await GetStats(id, cancellationToken);
            if (status != Status.Live)
            {
                throw new UpdateEntityException($"Entity with ID '{id}' is not live and cannot be updated.");
            }
        }
        return await UpdateAsync(entity, cancellationToken);
    }

    private async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Attach(entity);
        if (entity is IConcurrencyEntity obj) { obj.Revision++; }
        DbContext.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync(requestContext.UserId, cancellationToken);
        return entity;
    }
    public async Task<TEntity> DeleteAsync(TId id, CancellationToken cancellationToken)
    {
        TEntity entityToDelete = await GetAsync(id, cancellationToken);
        if (entityToDelete is IDeletableEntity obj) { obj.Status = Status.Deleted; }
        return await UpdateAsync(entityToDelete, cancellationToken);
    }
    public async Task<TEntity> PatchAsync(TId id, ICommandRequest<JsonPatchDocument<TEntity>> request, CancellationToken cancellationToken = default)
    {
        var patchDocument = request.Data ?? throw new UpdateEntityException("JsonPatchDocument<TEntity> is null");
        TEntity original = await GetAsync(id, cancellationToken);
        patchDocument.ApplyTo(original);
        return await UpdateAsync(original, cancellationToken);
    }
    public async Task<TEntity> ReplaceAsync(TId id, ICommandRequest<TEntity> request, CancellationToken cancellationToken)
    {
        var entity = request.Data ?? throw new UpdateEntityException("Entity is null");
        var sourceEntity = await GetAsync(id, cancellationToken);
        sourceEntity = sourceEntity ?? throw new UpdateEntityException("Id is invalid");
        DbContext.Entry(sourceEntity).CurrentValues.SetValues(entity);
        var modifyedCount = await DbContext.SaveChangesAsync(requestContext.UserId, cancellationToken);
        if (modifyedCount == 0) throw new UpdateEntityException("No records are replaced");
        return sourceEntity;
    }
}
