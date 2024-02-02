namespace SparkPlug.Api.Common;

public static class ExceptionWriter
{
    public static async ValueTask<bool> WriteInResponseAsync(HttpContext httpContext, Exception exception)
    {
        var details = new ProblemDetails()
        {
            Instance = httpContext.Request.Path,
            Status = httpContext.Response.StatusCode,
            Title = exception.Message
        };
        await httpContext.Response.WriteAsJsonAsync(details);
        return true;
    }
}