namespace SparkPlug.Api.Middleware;

public static class TransactionMiddlewareExtensions
{
    public static IApplicationBuilder UseTransactionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TransactionMiddleware>();
    }
}
