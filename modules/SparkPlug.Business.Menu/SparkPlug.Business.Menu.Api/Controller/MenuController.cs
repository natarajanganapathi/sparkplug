namespace SparkPlug.Business.Menu.Api;

[ApiController, Route("menu")]
public class MenuController : ApiController<long, MenuItem>
{
    public MenuController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}