namespace SparkPlug.Persistence.Abstractions;
public static class RepositoryExtentions
{
    public static TEntity Auditable<TId, TEntity>(this TEntity entity, TId userId, DateTime currentTime, bool isCreate = false) where TEntity : class, IBaseEntity<TId>
    {
        if (entity is IAuditableEntity<TId> obj)
        {
            if (isCreate)
            {
                obj.CreatedAt = currentTime;
                obj.CreatedBy = userId;
            }
            obj.ModifiedAt = DateTime.UtcNow;
            obj.ModifiedBy = userId;
        }
        return entity;
    }
}