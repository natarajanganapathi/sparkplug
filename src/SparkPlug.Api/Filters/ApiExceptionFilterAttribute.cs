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
        _logger.LogError(WebApiConstants.LogErrorMessageTemplate, context.Exception.Message, context.Exception);
        var response = context.HttpContext.Response;
        response.StatusCode = StatusCodes.Status500InternalServerError;
        response.ContentType = WebApiConstants.ContentType;
        context.Result = new JsonResult(new ErrorResponse("Internal Server Error", context.Exception));
        context.ExceptionHandled = true;
    }
}
