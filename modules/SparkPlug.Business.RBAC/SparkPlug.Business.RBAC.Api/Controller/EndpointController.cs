namespace SparkPlug.Business.RBAC.Api;

[ApiController, Route("endpoint"), ApiExplorerSettings(GroupName = "Template")]
public class EndpointController : BaseController<long, Resource>
{
    private EndpointService endpointService;
    public EndpointController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        endpointService = GetService<EndpointService>();
    }

    [HttpGet()]
    public IActionResult GetAsync(CancellationToken cancellationToken)
    {
        var entity = endpointService.GetAllEndpoints();
        return Ok(entity);
    }
}