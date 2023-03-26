namespace SparkPlug.Contracts;

[JsonConverter(typeof(FilterConverter))]
public class Filter : IFilter
{
    public Filter(FilterType filterType)
    {
        Kind = filterType;
    }
    public FilterType Kind { get; set; }
}
