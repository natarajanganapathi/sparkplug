namespace SparkPlug.Persistence.MongoDb.Repository;

public abstract class MongoRepository<TId, TEntity> : IRepository<TId, TEntity> where TEntity : BaseModel<TId>, new()
{
    internal readonly IMongoDbContext _context;
    internal readonly ILogger<MongoRepository<TId, TEntity>> _logger;
    private IMongoCollection<TEntity>? _collection;
    public virtual IMongoCollection<TEntity> Collection
    {
        get
        {
            if (_collection == null)
            {
                var collectionName = GetCollectionName(typeof(TEntity));
                _collection = _context.GetCollection<TEntity>(collectionName);
            }
            return _collection;
        }
    }
    private static String GetCollectionName(Type type)
    {
        var collectionName = type.GetCustomAttribute<CollectionAttribute>()?.Name;
        if (string.IsNullOrWhiteSpace(collectionName))
        {
            collectionName = typeof(TEntity).Name;
        }
        return collectionName;
    }
    protected MongoRepository(IMongoDbContext context, ILogger<MongoRepository<TId, TEntity>> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<IEnumerable<TEntity>> ListAsync(IQueryRequest? request)
    {
        var projection = GetProjection(request?.Select);
        var sort = GetSort(request?.Sort);
        var pc = request?.Page ?? new PageContext(1, 100);
        var filter = GetFilterDefinition(request?.Where);
        return await GetAsync(projection, filter, sort, pc);
    }
    public async Task<TEntity> GetAsync(TId id)
    {
        var filter = GetIdFilterDefinition(id);
        return await GetByFilter(filter).FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<TEntity>> GetAsync(ProjectionDefinition<TEntity>? projection, FilterDefinition<TEntity>? filter = default, SortDefinition<TEntity>? sorts = default, IPageContext? pc = default)
    {
        pc ??= new PageContext();
        filter ??= GetFilterBuilder().Empty;
        var query = GetByFilter(filter);
        if (projection != default)
        {
            query.Project(projection);
        }
        if (sorts != default)
        {
            query.Sort(sorts);
        }
        return await query.Skip(pc.Skip).Limit(pc.PageSize).ToListAsync();
    }
    public async Task<TEntity[]> GetManyAsync(TId[] ids)
    {
        var filter = GetFilterBuilder().In(x => x.Id, ids);
        var result = await GetByFilter(filter).ToListAsync();
        return result.ToArray();
    }
    public async Task<TEntity> CreateAsync(ICommandRequest<TEntity> request)
    {
        var entity = request.Data ?? throw new CreateEntityException("Entity is null");
        await Collection.InsertOneAsync(entity);
        return entity;
    }
    public async Task<TEntity[]> CreateManyAsync(ICommandRequest<TEntity[]> requests)
    {
        var entities = requests.Data ?? throw new CreateEntityException("Entities are null");
        await Collection.InsertManyAsync(entities);
        return entities;
    }
    public async Task<TEntity> UpdateAsync(TId id, ICommandRequest<TEntity> request)
    {
        id = id ?? throw new ArgumentNullException(nameof(id));
        var entity = request.Data ?? throw new UpdateEntityException("Entity is null");
        var filter = GetIdFilterDefinition(id);
        var update = GetUpdateDef(entity);
        await UpdateAsync(filter, update);
        return entity;
    }
    public async Task<UpdateResult> UpdateAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
    {
        var result = await Collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = false });
        return result;
    }
    public async Task<TEntity> PatchAsync(TId id, ICommandRequest<TEntity> request)
    {
        id = id ?? throw new ArgumentNullException(nameof(id));
        var entity = request.Data ?? throw new UpdateEntityException("Entity is null");
        var filter = GetIdFilterDefinition(id);
        var update = GetUpdateDef(entity, true);
        await UpdateAsync(filter, update);
        return entity;
    }

    public async Task<TEntity> ReplaceAsync(TId id, ICommandRequest<TEntity> request)
    {
        id = id ?? throw new ArgumentNullException(nameof(id));
        var entity = request.Data ?? throw new UpdateEntityException("Entity is null");
        await Collection.ReplaceOneAsync(GetIdFilterDefinition(id), entity);
        return entity;
    }
    public async Task<TEntity> DeleteAsync(TId id)
    {
        id = id ?? throw new DeleteEntityException("Id is null");
        var result = await Collection.DeleteOneAsync(GetIdFilterDefinition(id));
        return result.IsAcknowledged && result.DeletedCount > 0 ? new TEntity() : throw new DeleteEntityException($"Nothing is deleted. Id={id}");
    }
    public async Task<long> GetCountAsync(IQueryRequest request)
    {
        var filter = GetFilterDefinition(request.Where) ?? GetFilterBuilder().Empty;
        var result = Collection.Find(filter);
        return await result.CountDocumentsAsync().ConfigureAwait(false);
    }
    public UpdateDefinition<TEntity> GetUpdateDef(TEntity data, bool patch = false)
    {
        var properties = typeof(TEntity).GetProperties();
        List<UpdateDefinition<TEntity>> updates = new();
        for (var i = 0; i < properties.Length; i++)
        {
            var property = properties[i];
            if (property.Name.Equals(nameof(IBaseModel<TId>.Id), StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            var val = property.GetValue(data);
            if (patch && val == null)
            {
                continue;
            }
            updates.Add(GetUpdateBuilder().Set(property.Name, val));
        }
        return GetUpdateBuilder().Combine(updates);
    }
    public IFindFluent<TEntity, TEntity> GetByFilter(FilterDefinition<TEntity> filter)
    {
        return Collection.Find(filter);
    }
    public SortDefinitionBuilder<TEntity> GetSortBuilder()
    {
        return Builders<TEntity>.Sort;
    }
    public FilterDefinitionBuilder<TEntity> GetFilterBuilder()
    {
        return Builders<TEntity>.Filter;
    }
    public virtual FilterDefinition<TEntity> GetIdFilterDefinition(TId id)
    {
        return GetFilterBuilder().Eq("_id", id);
    }
    public UpdateDefinitionBuilder<TEntity> GetUpdateBuilder()
    {
        return Builders<TEntity>.Update;
    }
    public ProjectionDefinitionBuilder<TEntity> GetProjectionBuilder()
    {
        return Builders<TEntity>.Projection;
    }
    public PipelineDefinition<ChangeStreamDocument<BsonDocument>, BsonDocument> GetPipelineDefinition()
    {
        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<BsonDocument>>()
                .Match(x => x.OperationType == ChangeStreamOperationType.Insert || x.OperationType == ChangeStreamOperationType.Update || x.OperationType == ChangeStreamOperationType.Replace)
                .AppendStage<ChangeStreamDocument<BsonDocument>, ChangeStreamDocument<BsonDocument>, BsonDocument>("{ $project: { '_id': 1, 'fullDocument': 1, 'ns': 1, 'documentKey': 1 }}");
        return pipeline;
    }
    public ChangeStreamOptions GetChangeStreamOptions(ChangeStreamFullDocumentOption option)
    {
        return new ChangeStreamOptions { FullDocument = option };
    }
    public IChangeStreamCursor<BsonDocument> GetChangeStreamCursor(ChangeStreamFullDocumentOption fullDocOption = ChangeStreamFullDocumentOption.UpdateLookup)
    {
        var optons = GetChangeStreamOptions(fullDocOption);
        var pipeline = GetPipelineDefinition();
        return _context.GetCollection<BsonDocument>(GetCollectionName(typeof(TEntity))).Watch(pipeline, optons);
    }

    # region Query Builder
    private ProjectionDefinition<TEntity>? GetProjection(string[]? projection)
    {
        var projectionDefArray = projection?.Select(x => GetProjectionBuilder().Include(x)).ToArray();
        return projectionDefArray?.Length > 0 ? GetProjectionBuilder().Combine(projectionDefArray) : null;
    }
    private SortDefinition<TEntity>? GetSort(IOrder[]? orders)
    {
        var sortDef = orders != null ? GetSortDef(orders) : null;
        return sortDef?.Length > 0 ? GetSortBuilder().Combine(sortDef) : null;
    }
    private SortDefinition<TEntity>[] GetSortDef(IOrder[] orders)
    {
        var sortDef = new SortDefinition<TEntity>[orders.Length];
        for (int i = 0; i < orders.Length; i++)
        {
            var sort = orders[i];
            sortDef[i] = sort.Direction == Direction.Descending ? GetSortBuilder().Descending(sort.Field) : GetSortBuilder().Ascending(sort.Field);
        }
        return sortDef;
    }
    private FilterDefinition<TEntity>? GetFilterDefinition(IFilter? filter)
    {
        var builder = GetFilterBuilder();
        return filter == null ? builder.Empty : filter.GetFilterDefinition(builder);
    }
    # endregion
}

public static class Extention
{
    public static FilterDefinition<TEntity>[] GetFilterDefinitions<TEntity>(this IFilter[] filters, FilterDefinitionBuilder<TEntity> builder)
    {
        return filters.Select(x => x.GetFilterDefinition(builder)).ToArray();
    }
    public static FilterDefinition<TEntity> GetFilterDefinition<TEntity>(this IFilter filter, FilterDefinitionBuilder<TEntity> builder)
    {
        return filter switch
        {
            ICompositeFilter compositeFilter => compositeFilter.GetFilterDefinition(builder),
            IFieldFilter fieldFilter => fieldFilter.GetFilterDefinition(builder),
            IUnaryFilter unaryFilter => unaryFilter.GetFilterDefinition(builder),
            _ => throw new NotSupportedException($"Filter type {filter.GetType().Name} is not supported")
        };
    }

    public static FilterDefinition<TEntity> GetFilterDefinition<TEntity>(this ICompositeFilter compositeFilter, FilterDefinitionBuilder<TEntity> builder)
    {
        return compositeFilter.Op switch
        {
            CompositeOperator.And => builder.And(compositeFilter.Filters?.GetFilterDefinitions(builder)),
            CompositeOperator.Or => builder.Or(compositeFilter.Filters?.GetFilterDefinitions(builder)),
            _ => throw new QueryEntityException("Invalid composite filter operation")
        };
    }

    public static FilterDefinition<TEntity> GetFilterDefinition<TEntity>(this IFieldFilter fieldFilter, FilterDefinitionBuilder<TEntity> builder)
    {
        return fieldFilter.Op switch
        {
            FieldOperator.Equal => builder.Eq(fieldFilter.Field, fieldFilter.Value),
            FieldOperator.NotEqual => builder.Ne(fieldFilter.Field, fieldFilter.Value),
            FieldOperator.GreaterThan => builder.Gt(fieldFilter.Field, fieldFilter.Value),
            FieldOperator.GreaterThanOrEqual => builder.Gte(fieldFilter.Field, fieldFilter.Value),
            FieldOperator.LessThan => builder.Lt(fieldFilter.Field, fieldFilter.Value),
            FieldOperator.LessThanOrEqual => builder.Lte(fieldFilter.Field, fieldFilter.Value),
            FieldOperator.In => builder.In(fieldFilter.Field, fieldFilter.Value as IEnumerable<object>),
            FieldOperator.NotIn => builder.Nin(fieldFilter.Field, fieldFilter.Value as IEnumerable<object>),
            _ => throw new QueryEntityException("Invalid field filter operation")
        };
    }

    public static FilterDefinition<TEntity> GetFilterDefinition<TEntity>(this IUnaryFilter unaryFilter, FilterDefinitionBuilder<TEntity> builder)
    {
        return unaryFilter.Op switch
        {
            UnaryOperator.IsNull => builder.Eq(unaryFilter.Field, BsonNull.Value),
            UnaryOperator.IsNotNull => builder.Ne(unaryFilter.Field, BsonNull.Value),
            _ => throw new QueryEntityException("Invalid unary filter operation")
        };
    }
}