namespace SparkPlug.Business.RBAC.Api;

[ApiController, Route("permission")]
public class PermissionController : ApiController<long, Permission>
{
    public PermissionController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}