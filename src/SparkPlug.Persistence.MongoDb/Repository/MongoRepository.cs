namespace SparkPlug.Persistence.MongoDb;

public class MongoRepository<TId, TEntity> : IRepository<TId, TEntity> where TEntity : class, IBaseEntity<TId>, new()
{
    internal readonly MongoDbClient _context;
    internal readonly ILogger<MongoRepository<TId, TEntity>> _logger;
    internal readonly IRequestContext<TId> _requestContext;
    private IMongoCollection<TEntity>? _collection;
    public virtual IMongoCollection<TEntity> Collection
    {
        get => _collection ??= _context.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));
    }
    private static string GetCollectionName(Type type)
    {
        var collectionName = type.GetCustomAttribute<CollectionAttribute>()?.Name;
        if (string.IsNullOrWhiteSpace(collectionName))
        {
            collectionName = typeof(TEntity).Name;
        }
        return collectionName;
    }
    public MongoRepository(IServiceProvider serviceProvider)
    {
        _context = serviceProvider.GetRequiredService<MongoDbClient>();
        _logger = serviceProvider.GetRequiredService<ILogger<MongoRepository<TId, TEntity>>>();
        _requestContext = serviceProvider.GetRequiredService<IRequestContext<TId>>();
    }
    public async Task<IList<TEntity>> FindAsync(IQueryRequest request, CancellationToken cancellationToken)
    {
        // var pc = new PageContext();
        // var projection = GetProjection(request?.Select);
        // var sort = GetSort(request?.Sort);
        // var filter = GetFilterDefinition(request?.Where);
        // return await GetAsync(projection, filter, sort, pc, cancellationToken).ConfigureAwait(false);
        var pc = new PageContext(1, 100);
        request.Page ??= pc;
        var query = GetQuery(request);
        return await query.ToListAsync(cancellationToken);
        // return new PagedResult<TEntity>(data, await query.CountDocumentsAsync(cancellationToken));
    }
    public async Task<IList<JObject>> QueryAsync(IQueryRequest request, CancellationToken cancellationToken)
    {
        // var entitiesTask = GetData(request, cancellationToken);
        // var countTask = GetCountAsync(request, cancellationToken);
        // await Task.WhenAll(entitiesTask, countTask).ConfigureAwait(false);
        // return new ListResult<JObject>(entitiesTask.Result.Select(JObject.FromObject), countTask.Result);
        var pc = new PageContext(1, 100);
        request.Page ??= pc;
        var query = GetQuery(request);
        var data = await query.ToListAsync(cancellationToken);
        return data.ConvertAll(JObject.FromObject);
        // return new PagedResult<JObject>(data.Select(JObject.FromObject), await query.CountDocumentsAsync(cancellationToken));
    }

    public IFindFluent<TEntity, TEntity> GetQuery(IQueryRequest request)
    {
        var projection = GetProjection(request.Select);
        var filter = GetFilterDefinition(request.Where);
        var sort = GetSort(request.Sort);
        return GetFindFluent(projection, filter, sort, request.Page);
    }
    public async Task<long> CountAsync(IQueryRequest request, CancellationToken cancellationToken)
    {
        var filter = GetFilterDefinition(request.Where) ?? GetFilterBuilder().Empty;
        var result = Collection.Find(filter);
        return await result.CountDocumentsAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<TEntity> GetAsync(TId id, CancellationToken cancellationToken)
    {
        var filter = GetIdFilterDefinition(id);
        return await GetByFilter(filter).FirstOrDefaultAsync(cancellationToken);
    }
    public IFindFluent<TEntity, TEntity> GetFindFluent(ProjectionDefinition<TEntity>? projection, FilterDefinition<TEntity>? filter = default, SortDefinition<TEntity>? sorts = default, IPageContext? pc = default)
    {
        pc ??= new PageContext(1, 100);
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
        return query.Skip(pc.Skip).Limit(pc.PageSize);
    }
    public async Task<IEnumerable<TEntity>> GetManyAsync(TId[] ids, CancellationToken cancellationToken)
    {
        var filter = GetFilterBuilder().In(x => x.Id, ids);
        var result = await GetByFilter(filter).ToListAsync(cancellationToken);
        return result.ToArray();
    }
    public async Task<TEntity> CreateAsync(ICommandRequest<TEntity> request, CancellationToken cancellationToken)
    {
        var entity = request.Data ?? throw new CreateEntityException("Entity is null");
        await Collection.InsertOneAsync(entity.Auditable(_requestContext.UserId, DateTime.UtcNow, true), cancellationToken: cancellationToken);
        return entity;
    }
    public async Task<IEnumerable<TEntity>> CreateManyAsync(ICommandRequest<TEntity[]> request, CancellationToken cancellationToken)
    {
        var entities = request.Data ?? throw new CreateEntityException("Entities are null");
        await Collection.InsertManyAsync(entities.Select(x => x.Auditable(_requestContext.UserId, DateTime.UtcNow, true)), cancellationToken: cancellationToken);
        return entities;
    }
    public async Task<TEntity> UpdateAsync(TId id, ICommandRequest<TEntity> request, CancellationToken cancellationToken)
    {
        id = id ?? throw new ArgumentNullException(nameof(id));
        var entity = request.Data ?? throw new UpdateEntityException("Entity is null");
        entity = entity.Auditable(_requestContext.UserId, DateTime.UtcNow);
        var filter = GetIdFilterDefinition(id);
        var update = GetUpdateDef(entity);
        await UpdateAsync(filter, update);
        return entity;
    }
    public async Task<UpdateResult> UpdateAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
    {
        return await Collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = false });
    }
    public async Task<TEntity> PatchAsync(TId id, ICommandRequest<JsonPatchDocument<TEntity>> request, CancellationToken cancellationToken)
    {
        var patchDocument = request.Data ?? throw new UpdateEntityException("Entity is null");
        var filter = GetIdFilterDefinition(id);
        var entity = await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        patchDocument.ApplyTo(entity);
        await Collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        return entity;
    }

    public async Task<TEntity> ReplaceAsync(TId id, ICommandRequest<TEntity> request, CancellationToken cancellationToken)
    {
        id = id ?? throw new ArgumentNullException(nameof(id));
        var entity = request.Data ?? throw new UpdateEntityException("Entity is null");
        await Collection.ReplaceOneAsync(GetIdFilterDefinition(id), entity, cancellationToken: cancellationToken);
        return entity;
    }
    public async Task<TEntity> DeleteAsync(TId id, CancellationToken cancellationToken)
    {
        id = id ?? throw new DeleteEntityException("Id is null");
        var result = await Collection.DeleteOneAsync(GetIdFilterDefinition(id), cancellationToken);
        return result.IsAcknowledged && result.DeletedCount > 0 ? new TEntity() : throw new DeleteEntityException($"Nothing is deleted. Id={id}");
    }

    public UpdateDefinition<TEntity> GetUpdateDef(TEntity data, bool patch = false)
    {
        var properties = typeof(TEntity).GetProperties();
        List<UpdateDefinition<TEntity>> updates = new();
        for (var i = 0; i < properties.Length; i++)
        {
            var property = properties[i];
            if (property.Name.Equals(nameof(IBaseEntity<TId>.Id), StringComparison.OrdinalIgnoreCase))
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
        Type genericType = typeof(TId);
        ObjectId? oid = genericType == typeof(ObjectId)
        ? (ObjectId?)Convert.ChangeType(default(TId), genericType) : genericType == typeof(string)
        ? ObjectId.Parse(id as string) : throw new ArgumentException("Id type should be ObjectId or string");
        return GetFilterBuilder().Eq("_id", oid);
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
        return new EmptyPipelineDefinition<ChangeStreamDocument<BsonDocument>>()
                .Match(x => x.OperationType == ChangeStreamOperationType.Insert || x.OperationType == ChangeStreamOperationType.Update || x.OperationType == ChangeStreamOperationType.Replace)
                .AppendStage<ChangeStreamDocument<BsonDocument>, ChangeStreamDocument<BsonDocument>, BsonDocument>("{ $project: { '_id': 1, 'fullDocument': 1, 'ns': 1, 'documentKey': 1 }}");
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
    #endregion
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