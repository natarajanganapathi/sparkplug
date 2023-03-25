namespace SparkPlug.Contracts;

public class CommandResponse : ApiResponse, ICommandResponse
{
    public CommandResponse(JObject data)
    {
        Data = data;
    }
    public CommandResponse(object? data = default)
    {
        Data = data == null ? new JObject() : JObject.FromObject(data);
    }
    public JObject Data { get; set; }
}