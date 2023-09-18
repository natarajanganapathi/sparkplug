namespace SparkPlug.Business.Tenancy.Service;

public class TenantResolverMiddleware
{
    private const string Tenant = "tenant";
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _cache;
    private readonly ISerializer _serializer;

    public TenantResolverMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _cache = serviceProvider.GetRequiredService<IDistributedCache>();
        _serializer = serviceProvider.GetRequiredService<ISerializer>();
    }

    public async Task InvokeAsync(HttpContext context, ITenantResolver tenantResolver)
    {
        if (context.Request.RouteValues.ContainsKey(Tenant))
        {
            string tenantId = GetTenantIdFromRequest(context.Request);
            context.Items["Tenant"] = await GetTenantFromCacheOrResolver(context, tenantResolver, tenantId);
        }
        await _next(context);
    }

    private static string GetTenantIdFromRequest(HttpRequest request)
    {
        var tenantId = request.RouteValues[Tenant]?.ToString();
        if (string.IsNullOrWhiteSpace(tenantId)) { throw new ArgumentException("Tenant identifier missed in the url"); }
        return tenantId;
    }

    private async Task<ITenant> GetTenantFromCacheOrResolver(HttpContext context, ITenantResolver tenantResolver, string? tenantId)
    {
        string key = $"{Tenant}_{tenantId ?? "default"}";
        var cachedTenant = await _cache.GetAsync(key, context.RequestAborted);
        ITenant? tenant = cachedTenant == null ? null : ConvertTo<Tenant>(cachedTenant);
        if (tenant == null)
        {
            tenant = await tenantResolver.ResolveAsync(tenantId);
            if (tenant != null)
            {
                var options = new DistributedCacheEntryOptions()
                                .SetSlidingExpiration(TimeSpan.FromMinutes(10)); //TODO: 
                await _cache.SetAsync(key, Encoding.UTF8.GetBytes(_serializer.Serialize(tenant)), options);
            }
        }
        return tenant!;
    }

    private T? ConvertTo<T>(byte[] content)
    {
        var tenantJson = Encoding.UTF8.GetString(content);
        return _serializer.Deserialize<T>(tenantJson);
    }
}
