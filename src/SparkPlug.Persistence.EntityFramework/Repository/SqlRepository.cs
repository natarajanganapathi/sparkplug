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
    public async Task<ListResult<JObject>> QueryAsync(IQueryRequest? request, CancellationToken cancellationToken)
    {
        var query = new QueryBuilder<TEntity>(DbSet, request).Project();
#if DEBUG
        logger.LogInformation("Search Query: {query}", query.ToQueryString());
#endif
        return new ListResult<JObject>(await query.ToListAsync(cancellationToken), await query.LongCountAsync(cancellationToken));
    }
    public async Task<IEnumerable<TEntity>> FindAsync(IQueryRequest? request, CancellationToken cancellationToken)
    {
        return await new QueryBuilder<TEntity>(DbSet, request).Select().ToListAsync(cancellationToken);
    }

    public async Task<long> GetCountAsync(IQueryRequest? request, CancellationToken cancellationToken)
    {
        return await new QueryBuilder<TEntity>(DbSet, request).Count(cancellationToken);
    }
    public async Task<TEntity> GetAsync(TId id, CancellationToken cancellationToken)
    {
        var tid = id ?? throw new QueryEntityException("Id is null");
        var result = await DbSet.FindAsync(new object[] { tid }, cancellationToken).ConfigureAwait(false);
        return result ?? throw new QueryEntityException("Id is not found");
    }
    public async Task<IEnumerable<TEntity>> GetManyAsync(TId[] ids, CancellationToken cancellationToken)
    {
        if (ids == null) throw new QueryEntityException("Ids are null or empty");
        return await DbSet.Where(x => ids.Contains(x.Id)).ToArrayAsync(cancellationToken);
    }
    public async Task<TEntity> CreateAsync(ICommandRequest<TEntity> request, CancellationToken cancellationToken)
    {
        var entity = request.Data ?? throw new CreateEntityException("Entity is null");
        var entityEntry = await DbSet.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(requestContext.UserId, cancellationToken);
        return entityEntry.Entity;
    }
    public async Task<IEnumerable<TEntity>> CreateManyAsync(ICommandRequest<TEntity[]> request, CancellationToken cancellationToken)
    {
        var entities = request.Data ?? throw new CreateEntityException("Entities are null");
        await DbSet.AddRangeAsync(entities, cancellationToken);
        await DbContext.SaveChangesAsync(requestContext.UserId, cancellationToken);
        return entities;
    }
    public async Task<TEntity> UpdateAsync(TId id, ICommandRequest<TEntity> request, CancellationToken cancellationToken)
    {
        var entity = request.Data ?? throw new UpdateEntityException("Entity is null");
        entity.Id = id ?? throw new UpdateEntityException("Id is null");
        return await UpdateAsync(entity, cancellationToken);
    }
    private async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        DbSet.Attach(entity);
        if (entity is IConcurrencyEntity obj) { obj.Revision++; }
        DbContext.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync(requestContext.UserId, cancellationToken);
        return entity;
    }
    public async Task<TEntity> DeleteAsync(TId id, CancellationToken cancellationToken)
    {
        var tid = id ?? throw new DeleteEntityException("Id is null");
        TEntity entityToDelete = (await DbSet.FindAsync(new object[] { tid }, cancellationToken).ConfigureAwait(false)) ?? throw new DeleteEntityException("Id is invalid");
        if (entityToDelete is IDeletableEntity obj) { obj.Status = Status.Deleted; }
        return await UpdateAsync(entityToDelete, cancellationToken);
    }

    public async Task<TEntity> PatchAsync(TId id, ICommandRequest<JsonPatchDocument<TEntity>> request, CancellationToken cancellationToken)
    {
        var patchDocument = request.Data ?? throw new UpdateEntityException("Entity is null");
        var tid = id ?? throw new UpdateEntityException("Id is null");
        TEntity original = await GetAsync(tid, cancellationToken);
        patchDocument.ApplyTo(original);
        return await UpdateAsync(original, cancellationToken);
    }
    public async Task<TEntity> ReplaceAsync(TId id, ICommandRequest<TEntity> request, CancellationToken cancellationToken)
    {
        var entity = request.Data ?? throw new UpdateEntityException("Entity is null");
        var tid = id ?? throw new UpdateEntityException("Id is null");
        var sourceEntity = await GetAsync(tid, cancellationToken).ConfigureAwait(false);
        sourceEntity = sourceEntity ?? throw new UpdateEntityException("Id is invalid");
        DbContext.Entry(sourceEntity).CurrentValues.SetValues(entity);
        var modifyedCount = await DbContext.SaveChangesAsync(requestContext.UserId, cancellationToken);
        if (modifyedCount == 0) throw new UpdateEntityException("No records are replaced");
        return sourceEntity;
    }
}
