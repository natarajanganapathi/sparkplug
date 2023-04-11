namespace SparkPlug.Persistence.Abstractions;

public record ListResult<TResult>
{
    public ListResult(IEnumerable<TResult> items, long count)
    {
        Items = items;
        Count = count;
    }
    public IEnumerable<TResult> Items { get; set; }
    public long Count { get; set; }
}