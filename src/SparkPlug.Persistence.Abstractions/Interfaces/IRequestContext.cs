namespace SparkPlug.Persistence.Abstractions;

public interface IRequestContext<out TId>
{
    public TId UserId { get; }
}