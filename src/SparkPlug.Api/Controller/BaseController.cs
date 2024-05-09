namespace SparkPlug.Api.Controllers;

public abstract class BaseController<TId, TEntity> : ControllerBase where TEntity : class, IBaseEntity<TId>, new()
{
    private readonly IServiceProvider _serviceProvider;
    protected readonly BaseService<TId, TEntity> Service;
    protected readonly ILogger<BaseController<TId, TEntity>> Logger;

    protected BaseController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Service = serviceProvider.GetRequiredService<BaseService<TId, TEntity>>();
        Logger = serviceProvider.GetRequiredService<ILogger<BaseController<TId, TEntity>>>();
    }
    public TService GetService<TService>() where TService : class
                 => _serviceProvider.GetRequiredService<TService>();

    [NonAction]
    public OkObjectResult Ok([ActionResultObjectValue] IEnumerable<TEntity>? data, [ActionResultObjectValue] IPageContext? pagecontext)
    {
        return Ok(new QueryResponse(data, pagecontext));
    }
    [NonAction]
    public OkObjectResult Ok([ActionResultObjectValue] IEnumerable<JObject> data, [ActionResultObjectValue] IPageContext? pagecontext)
    {
        return Ok(new QueryResponse(data, pagecontext));
    }
    [NonAction]
    public OkObjectResult Ok([ActionResultObjectValue] TEntity data)
    {
        return Ok(new CommandResponse(data));
    }
}
