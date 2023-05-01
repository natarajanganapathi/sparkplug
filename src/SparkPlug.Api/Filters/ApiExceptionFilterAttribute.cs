namespace SparkPlug.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    public override void OnException(ExceptionContext exceptionContext)
    {
        _logger.LogError(WebApiConstants.LogErrorMessageTemplate, exceptionContext.Exception.Message, exceptionContext.Exception);
        var errorResponse = new ErrorResponse().SetFromException(exceptionContext.Exception);
        if (_httpContextAccessor.HttpContext != null)
        {
            errorResponse.SetTraceIdentifier(_httpContextAccessor.HttpContext.TraceIdentifier);
        }
        var response = exceptionContext.HttpContext.Response;
        response.StatusCode = StatusCodes.Status500InternalServerError;
        response.ContentType = WebApiConstants.ContentType;
        exceptionContext.Result = new JsonResult(errorResponse);
        exceptionContext.ExceptionHandled = true;
    }
}
