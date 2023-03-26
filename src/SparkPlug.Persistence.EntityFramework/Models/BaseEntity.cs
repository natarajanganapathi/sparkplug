namespace SparkPlug.Persistence.EntityFramework.Models;

public abstract class BaseEntity<TId> : IBaseEntity<TId> where TId : new()
{
    public virtual TId Id { get; set; } = new();
    public virtual TId GetId()
    {
        return Id;
    }
}
