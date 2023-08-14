namespace SparkPlug.Contracts;

public class QueryResponse : ApiResponse, IQueryResponse
{
    public QueryResponse(IEnumerable<object>? data = default, IPageContext? pc = default)
    {
        Data = data;
        Page = pc;
    }
    public IPageContext? Page { get; set; }
    public IEnumerable<object>? Data { get; set; }
}

public static partial class Extensions
{
    #region QueryResponse
    public static IQueryResponse AddData(this IQueryResponse source, IEnumerable<object> data)
    {
        source.Data = data;
        return source;
    }
    public static IQueryResponse AddPageContext(this IQueryResponse source, IPageContext pc)
    {
        source.Page = pc;
        return source;
    }
    #endregion
}