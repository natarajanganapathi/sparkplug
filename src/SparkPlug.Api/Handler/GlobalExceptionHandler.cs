namespace SparkPlug.Api.Handler;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        return await ExceptionWriter.WriteInResponseAsync(httpContext, exception);
    }
}