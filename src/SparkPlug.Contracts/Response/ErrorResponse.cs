namespace SparkPlug.Contracts;

public class ErrorResponse : ApiResponse, IErrorResponse
{
    public string? Message { get; set; }
    public string? Code { get; set; }
    public string TraceIdentifier { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
}

public static class ExceptionExtension
{
    public static ErrorResponse SetMessage(this ErrorResponse error, string message)
    {
        error.Message = message;
        return error;
    }
    public static ErrorResponse SetCode(this ErrorResponse error, string code)
    {
        error.Code = code;
        return error;
    }
    public static ErrorResponse SetStackTrace(this ErrorResponse error, string stackTrace)
    {
#if DEBUG
        error.StackTrace = stackTrace;
#endif
        return error;
    }
    public static ErrorResponse SetTraceIdentifier(this ErrorResponse error, string traceIdentifier)
    {
        error.TraceIdentifier = traceIdentifier;
        return error;
    }
    public static ErrorResponse SetFromException(this ErrorResponse error, Exception ex)
    {
        error.SetMessage(ex.Message);
        error.SetStackTrace(ex.GetInnerStackTrace());
        return error;
    }
    public static string GetInnerStackTrace(this Exception? exception)
    {
        var sb = new StringBuilder();
        int level = 0;
        while (exception != null)
        {
            sb = sb.Append("Level: ").Append(level++)
                .Append(", Exception: ")
                .Append(exception.Message)
                .Append(", StatckTrace: ")
                .Append(exception.StackTrace)
                .Append(Environment.NewLine);
            exception = exception.InnerException;
        }
        return sb.ToString();
    }
}
