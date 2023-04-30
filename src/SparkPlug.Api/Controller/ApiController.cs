namespace SparkPlug.Api.Controllers;

[ApiExplorerSettings(GroupName = "Template")]
public abstract class ApiController<TId, TEntity> : BaseController<TId, TEntity> where TEntity : class, IBaseEntity<TId>, new()
{
    protected ApiController(IServiceProvider serviceProvider) : base(serviceProvider) { }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int? pageNo, [FromQuery] int? pageSize, CancellationToken cancellationToken)
    {
        var pc = new PageContext(pageNo ?? 1, pageSize ?? 25);
        var request = new QueryRequest(pc);
        var data = await _service.FindAsync(request, cancellationToken);
        var count = await _service.CountAsync(request, cancellationToken);
        return Ok(data, pc.SetTotal(count));
    }

    [HttpPost("search")]
    public async Task<IActionResult> Query([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        request.Page ??= new PageContext();
        var data = await _service.QueryAsync(request, cancellationToken);
        var count = await _service.CountAsync(request, cancellationToken);
        return Ok(data, request.Page.SetTotal(count));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CommandRequest<TEntity> rec, CancellationToken cancellationToken)
    {
        var entity = await _service.CreateAsync(rec, cancellationToken);
        return Ok(entity);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(TId id, CancellationToken cancellationToken)
    {
        var entity = await _service.GetAsync(id, cancellationToken);
        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(TId id, [FromBody] CommandRequest<TEntity> rec, CancellationToken cancellationToken)
    {
        var entity = await _service.UpdateAsync(id, rec, cancellationToken);
        return Ok(entity);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(TId id, [FromBody] CommandRequest<JsonPatchDocument<TEntity>> rec, CancellationToken cancellationToken)
    {
        var entity = await _service.PatchAsync(id, rec, cancellationToken);
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(TId id, CancellationToken cancellationToken)
    {
        var entity = await _service.DeleteAsync(id, cancellationToken);
        return Ok(entity);
    }
}
