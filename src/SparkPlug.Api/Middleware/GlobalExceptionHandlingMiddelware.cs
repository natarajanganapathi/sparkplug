namespace SparkPlug.Api.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            context.Response.Headers.Add("X-Trace-Id", context.TraceIdentifier);
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var eventId = new EventId((int)ApiEventId.UnknownGlobaException, nameof(ApiEventId.UnknownGlobaException));
        _logger.LogError(eventId, exception, $"Trace-Id: {context.TraceIdentifier} Message: {exception.Message}");
        var response = context.Response;
        response.StatusCode = StatusCodes.Status500InternalServerError;
        response.ContentType = WebApiConstants.ContentType;
        var errorResponse = new ErrorResponse().SetFromException(exception).SetCode($"{eventId.Id}");
        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}
