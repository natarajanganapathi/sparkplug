namespace SparkPlug.Menu.Service;

public class MenuService : BaseService<long, MenuItem>
{
   public MenuService(IServiceProvider serviceProvider) : base(serviceProvider) { }
}