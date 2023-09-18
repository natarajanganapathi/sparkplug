namespace SparkPlug.Business.RBAC.Service;

public class ResourceService : BaseService<long, Resource>
{
   public ResourceService(IServiceProvider serviceProvider) : base(serviceProvider) { }
}