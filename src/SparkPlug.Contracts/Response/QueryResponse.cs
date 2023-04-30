namespace SparkPlug.Contracts;

public class QueryResponse : ApiResponse, IQueryResponse
{
    public QueryResponse(JArray? data = default, IPageContext? pc = null)
    {
        Data = data;
        Page = pc;
    }
    public QueryResponse(object? data, IPageContext? pc)
    {
        Data = data == null ? new JArray() : JArray.FromObject(data);
        Page = pc;
    }
    public IPageContext? Page { get; set; }
    public JArray? Data { get; set; }
}

public static partial class Extensions
{
    #region QueryResponse
    public static IQueryResponse AddResponse(this IQueryResponse source, JArray data)
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