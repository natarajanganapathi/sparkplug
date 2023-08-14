namespace SparkPlug.Contracts;

public class CommandResponse : ApiResponse, ICommandResponse
{
    public CommandResponse(object? data = default)
    {
        Data = data;
    }
    public object? Data { get; set; }
}