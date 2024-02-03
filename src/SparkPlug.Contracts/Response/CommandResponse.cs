namespace SparkPlug.Contracts;

public record CommandResponse : ApiResponse, ICommandResponse
{
    public CommandResponse(object? data = default)
    {
        Data = data;
    }
    public object? Data { get; set; }
}