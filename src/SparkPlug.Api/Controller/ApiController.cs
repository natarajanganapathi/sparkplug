namespace SparkPlug.Api.Controllers;

[ApiExplorerSettings(GroupName = "Template")]
public sealed class ApiController<TId, TEntity> : BaseController<TId, TEntity> where TEntity : class, IBaseEntity<TId>, new()
{
    public ApiController(IServiceProvider serviceProvider) : base(serviceProvider) { }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int? pageNo, [FromQuery] int? pageSize, CancellationToken cancellationToken)
    {
        var request = new QueryRequest(new PageContext(pageNo ?? 1, pageSize ?? 25));
        var result = await _repository.FindAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.QueryAsync(request, cancellationToken);
        var pc = request?.Page ?? new PageContext();
        return Ok(result.Items, pc.SetTotal(result.Count));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CommandRequest<TEntity> rec, CancellationToken cancellationToken)
    {
        var entity = await _repository.CreateAsync(rec, cancellationToken);
        return Ok(entity);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(TId id, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(id, cancellationToken);
        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(TId id, [FromBody] CommandRequest<TEntity> rec, CancellationToken cancellationToken)
    {
        var entity = await _repository.UpdateAsync(id, rec, cancellationToken);
        return Ok(entity);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(TId id, [FromBody] CommandRequest<JsonPatchDocument<TEntity>> rec, CancellationToken cancellationToken)
    {
        var entity = await _repository.PatchAsync(id, rec, cancellationToken);
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(TId id, CancellationToken cancellationToken)
    {
        var entity = await _repository.DeleteAsync(id, cancellationToken);
        return Ok(entity);
    }
}
