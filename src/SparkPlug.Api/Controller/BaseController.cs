namespace SparkPlug.Api.Controllers;

public abstract class BaseController<TId, TEntity> : ControllerBase where TEntity : class, IBaseEntity<TId>, new()
{
    protected readonly Repository<TId, TEntity> _repository;
    protected readonly ILogger<BaseController<TId, TEntity>> _logger;
    protected readonly IServiceProvider _serviceProvider;

    protected BaseController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _repository = serviceProvider.GetRequiredService<Repository<TId, TEntity>>();
        _logger = serviceProvider.GetRequiredService<ILogger<BaseController<TId, TEntity>>>();
    }
    [NonAction]
    public OkObjectResult Ok([ActionResultObjectValue] IEnumerable<TEntity> data, [ActionResultObjectValue] IPageContext? pagecontext)
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
