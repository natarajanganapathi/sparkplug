namespace SparkPlug.Persistence.Abstractions;

public record PagedResult<TResult>
{
    public PagedResult(IEnumerable<TResult> items, long count)
    {
        Items = items;
        Count = count;
    }
    public IEnumerable<TResult> Items { get; set; }
    public long Count { get; set; }
}