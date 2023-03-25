namespace SparkPlug.Persistence.Abstractions;

public interface IBaseEntity<TId>
{
    public TId Id { get; set; }
    public TId GetId();
}