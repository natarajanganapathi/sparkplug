namespace SparkPlug.Business.RBAC.Api;

[ApiController, Route("resource")]
public class ResourceController : ApiController<long, Resource>
{
    public ResourceController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}