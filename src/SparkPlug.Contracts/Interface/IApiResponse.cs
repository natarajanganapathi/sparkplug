namespace SparkPlug.Contracts;

public interface IApiResponse { }

public interface IErrorResponse : IApiResponse
{
    string? Code { get; set; }
    string? Message { get; set; }
    string? StackTrace { get; set; }
    string TraceIdentifier { get; set; }
}

public interface IQueryResponse : IApiResponse
{
    IPageContext? Page { get; set; }
    JArray? Data { get; set; }
}

public interface ICommandResponse : IApiResponse
{
    JObject Data { get; set; }
}

public interface ICompositeResponse : IApiResponse
{
    Dictionary<string, IApiResponse>? Data { get; set; }
}
