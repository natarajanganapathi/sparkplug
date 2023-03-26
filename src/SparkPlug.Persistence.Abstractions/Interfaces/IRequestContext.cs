namespace SparkPlug.Persistence.Abstractions;

public interface IRequestContext<TId>
{
    public TId UserId { get; }
}