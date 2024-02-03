namespace SparkPlug.Business.Tenancy.Service;

public class TenantResolverMiddleware
{
    private const string Tenant = "tenant";
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _distributedCache;
    private readonly ISerializer _serializer;

    public TenantResolverMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _distributedCache = serviceProvider.GetRequiredService<IDistributedCache>();
        _serializer = serviceProvider.GetRequiredService<ISerializer>();
    }

    public async Task InvokeAsync(HttpContext httpContext, ITenantResolver tenantResolver)
    {
        if (httpContext.Request.RouteValues.ContainsKey(Tenant))
        {
            httpContext.Items["Tenant"] = await GetTenant(httpContext, tenantResolver);
        }
        await _next(httpContext);
    }
    private async Task<ITenant?> GetTenant(HttpContext httpContext, ITenantResolver tenantResolver)
    {
        string tenantId = GetTenantIdFromRequest(httpContext.Request);
        return await GetTenantFromCacheAsync(tenantId, httpContext) ?? await ResolveAndSetCacheAsync(tenantId, tenantResolver, httpContext.RequestAborted);
    }
    private static string GetTenantIdFromRequest(HttpRequest request)
    {
        var tenantId = request.RouteValues[Tenant]?.ToString();
        if (string.IsNullOrWhiteSpace(tenantId)) { throw new ArgumentException("Tenant identifier missed in the url"); }
        return tenantId;
    }
    private async Task<ITenant?> GetTenantFromCacheAsync(string? tenantId, HttpContext httpContext)
    {
        var cachedTenant = await _distributedCache.GetAsync(GetCacheKey(tenantId), httpContext.RequestAborted);
        return cachedTenant is null ? null : ConvertTo<Tenant>(cachedTenant);
    }
    private string GetCacheKey(string? tenantId)
    {
        return $"{Tenant}_{tenantId ?? "default"}";
    }
    private T? ConvertTo<T>(byte[] content)
    {
        var tenantJson = Encoding.UTF8.GetString(content);
        return _serializer.Deserialize<T>(tenantJson);
    }
    private async Task<ITenant?> ResolveAndSetCacheAsync(string tenantId, ITenantResolver tenantResolver, CancellationToken cancellationToken)
    {
        var tenant = await tenantResolver.ResolveAsync(tenantId, cancellationToken);
        return tenant == null ? tenant : await SetCacheAsync(GetCacheKey(tenantId), tenant);
    }
    private async Task<ITenant> SetCacheAsync(string key, ITenant tenant)
    {
        var options = new DistributedCacheEntryOptions()
                                        .SetSlidingExpiration(TimeSpan.FromMinutes(10)); //TODO: 
        await _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(_serializer.Serialize(tenant)), options);
        return tenant;
    }
}
