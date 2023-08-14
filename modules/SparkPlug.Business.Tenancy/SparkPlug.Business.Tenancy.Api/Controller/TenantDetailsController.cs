namespace SparkPlug.Business.Tenancy.Api;

[ApiController, Route("tenant-details")]
public class TenantDetailsController : ApiController<long, TenantDetails>
{
    public TenantDetailsController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}