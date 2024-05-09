namespace SparkPlug.Api.Controllers;

[ApiExplorerSettings(GroupName = "Template")]
public abstract class ApiController<TId, TEntity> : BaseController<TId, TEntity> where TEntity : class, IBaseEntity<TId>, new()
{
    protected ApiController(IServiceProvider serviceProvider) : base(serviceProvider) { }

    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery] int? pageNo, [FromQuery] int? pageSize, CancellationToken cancellationToken)
    {
        var pc = new PageContext(pageNo ?? 1, pageSize ?? 25);
        var request = new QueryRequest(pc);
        var data = await Service.FindAsync(request, cancellationToken);
        var count = await Service.CountAsync(request, cancellationToken);
        return Ok(data, pc.SetTotal(count));
    }

    [HttpPost("search")]
    public async Task<IActionResult> QueryAsync([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        request.Page ??= new PageContext();
        var data = await Service.QueryAsync(request, cancellationToken);
        var count = await Service.CountAsync(request, cancellationToken);
        return Ok(data, request.Page.SetTotal(count));
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CommandRequest<TEntity> rec, CancellationToken cancellationToken)
    {
        var entity = await Service.CreateAsync(rec, cancellationToken);
        return Ok(entity);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(TId id, CancellationToken cancellationToken)
    {
        var entity = await Service.GetAsync(id, cancellationToken);
        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(TId id, [FromBody] CommandRequest<TEntity> rec, CancellationToken cancellationToken)
    {
        var entity = await Service.UpdateAsync(id, rec, cancellationToken);
        return Ok(entity);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchAsync(TId id, [FromBody] CommandRequest<JsonPatchDocument<TEntity>> rec, CancellationToken cancellationToken)
    {
        var entity = await Service.PatchAsync(id, rec, cancellationToken);
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(TId id, CancellationToken cancellationToken)
    {
        var entity = await Service.DeleteAsync(id, cancellationToken);
        return Ok(entity);
    }
}
