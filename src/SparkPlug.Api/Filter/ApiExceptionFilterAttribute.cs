namespace SparkPlug.Api.Filter;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override async void OnException(ExceptionContext exceptionContext)
    {
        var isHandled = await ExceptionWriter.WriteInResponseAsync(exceptionContext.HttpContext, exceptionContext.Exception); ;
        exceptionContext.ExceptionHandled = isHandled;
    }
}
