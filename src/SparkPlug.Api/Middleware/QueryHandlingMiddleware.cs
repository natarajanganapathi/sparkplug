namespace SparkPlug.Api.Middleware;

public class QueryHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public QueryHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!string.IsNullOrEmpty(context.Request.QueryString.Value))
        {
            var query = context.Request.QueryString.Value;
            var decodedQuery = DecodeBase64QueryString(query);

            if (!string.IsNullOrEmpty(decodedQuery))
            {
                context.Request.QueryString = new QueryString(decodedQuery);
            }
        }

        await _next(context);
    }

    private static string DecodeBase64QueryString(string query)
    {
        var queryWithoutQuestionMark = query.TrimStart('?');
        var bytes = Convert.FromBase64String(queryWithoutQuestionMark);
        return Encoding.UTF8.GetString(bytes);
    }
}
