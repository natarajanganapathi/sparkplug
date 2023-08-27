namespace SparkPlug.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
    {
        _logger = logger;
    }
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var eventId = new EventId((int)ApiEventId.ExceptionFilter, nameof(ApiEventId.ExceptionFilter));
        _logger.LogError(eventId, exception, "{XTraceId}: {TraceIdentifier} Message: {Message}", Constants.XTraceId, context.HttpContext?.TraceIdentifier, exception.Message);

        if (context.HttpContext != null)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            response.ContentType = Constants.JsonContentType;
        }
        var errorResponse = new ErrorResponse().SetFromException(exception);
        context.Result = new JsonResult(errorResponse);
        context.ExceptionHandled = true;
    }
}
