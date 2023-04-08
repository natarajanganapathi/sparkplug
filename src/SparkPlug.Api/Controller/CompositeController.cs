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
        if (request != null && request.Requests != null)
        {
            foreach (var kv in request.Requests)
            {
                var payload = JsonConvert.SerializeObject(kv.Value);
                var httpClient = _httpClientFactory.CreateClient();
                var requestContent = new StringContent(payload, Encoding.UTF8, "application/json");
                // If kv.Value is QueryRequest crate GetAsync and CommandRequest create PostAsync
                var response = await httpClient.PostAsync(kv.Key, requestContent, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    res.Add(kv.Key, new ErrorResponse(message: $"API call failed with status code {response.StatusCode}"));
                }
                var responseContent = await response.Content.ReadAsStringAsync();
            }
        }
        return Ok(new CompositeResponse(data: res));
    }
    // public CompositeController(IServiceProvider serviceProvider) : base(serviceProvider) { }

    // [HttpPost]
    // public async Task<IActionResult> MyMethod(TEntity myModel)
    // {
    //     string apiUrl = "/api/MyController/MyMethod";

    //     JsonSerializer.Serialize(myModel);
    //     var requestContent = new StringContent("", Encoding.UTF8, "application/json");

    //     var response = await _httpClient.PostAsync(apiUrl, requestContent);

    //     if (!response.IsSuccessStatusCode)
    //     {
    //         return BadRequest();
    //     }

    //     var responseContent = await response.Content.ReadAsStringAsync();
    //     var responseObject = JsonSerializer.Deserialize<MyResponseModel>(responseContent);

    //     return Ok(responseObject);
    // }

    // [HttpPost("search")]
    // public async Task<IActionResult> Search([FromBody] CompositeRequest request, CancellationToken cancellationToken)
    // {
    //     const string apiUrl = "api/MyController/MyMethod";
    //     HttpContent requestContent = new StringContent("{ \"param1\": \"value1\", \"param2\": 123 }", Encoding.UTF8, "application/json");

    //     // Create the request message
    //     HttpRequestMessage requestMessage = new(HttpMethod.Post, apiUrl)
    //     {
    //         Content = requestContent
    //     };

    //     // Get the HttpConfiguration and HttpServer
    //     HttpConfiguration config = GlobalConfiguration.Configuration;
    //     HttpServer server = new HttpServer(config);

    //     // Create the HttpMessageInvoker and send the request
    //     HttpMessageInvoker invoker = new(server);
    //     HttpResponseMessage response = await invoker.SendAsync(requestMessage, cancellationToken);

    //     // Read the response content
    //     string responseContent = response.Content.ReadAsStringAsync().Result;

    //     throw new NotImplementedException();
    // }
}