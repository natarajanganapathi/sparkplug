namespace SparkPlug.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;
    private readonly HttpContext _context;
    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger, HttpContext context)
    {
        _logger = logger;
        _context = context;
    }
    public override void OnException(ExceptionContext exceptionContext)
    {
        _logger.LogError(WebApiConstants.LogErrorMessageTemplate, exceptionContext.Exception.Message, exceptionContext.Exception);
        var response = exceptionContext.HttpContext.Response;
        response.StatusCode = StatusCodes.Status500InternalServerError;
        response.ContentType = WebApiConstants.ContentType;
        exceptionContext.Result = new JsonResult(new ErrorResponse().SetFromException(exceptionContext.Exception).SetTraceIdentifier(_context.TraceIdentifier));
        exceptionContext.ExceptionHandled = true;
    }
}
