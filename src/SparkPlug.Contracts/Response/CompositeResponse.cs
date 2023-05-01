namespace SparkPlug.Contracts;

public class CompositeResponse : ApiResponse, ICompositeResponse
{
    public CompositeResponse(Dictionary<string, IApiResponse>? data = null)
    {
        Data = data;
    }
    public Dictionary<string, IApiResponse>? Data { get; set; }
}