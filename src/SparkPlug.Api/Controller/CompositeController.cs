namespace SparkPlug.Api.Controllers;

[ApiController, Route("composite"), ApiExplorerSettings(GroupName = "v1")]
public class CompositeController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CompositeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] CompositeRequest request, CancellationToken cancellationToken)
    {
        var res = new Dictionary<string, IApiResponse>();
        if (request?.Requests != null)
        {
            foreach (var kv in request.Requests)
            {
                var payload = JsonConvert.SerializeObject(new object()); // kv.Value
                var httpClient = _httpClientFactory.CreateClient();
                var requestContent = new StringContent(payload, Encoding.UTF8, "application/json");
                // If kv.Value is QueryRequest crate GetAsync and CommandRequest create PostAsync
                var response = await httpClient.PostAsync(kv.Key, requestContent, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    res.Add(kv.Key, new ErrorResponse().SetMessage($"API call failed with status code {response.StatusCode}"));
                }
                _ = await response.Content.ReadAsStringAsync(cancellationToken);
            }
        }
        return Ok(new CompositeResponse(data: res));
    }
}