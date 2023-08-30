namespace SparkPlug.Business.RBAC.Service;

public class PermissionService : BaseService<long, Permission>
{
   public PermissionService(IServiceProvider serviceProvider) : base(serviceProvider) { }
}