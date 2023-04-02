namespace SparkPlug.Api.Middleware;

public class TenantResolverMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolverMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantResolver tenantResolver)
    {
        var tenantId = context.GetRouteValue(WebApiConstants.Tenant)?.ToString();
        context.Items["Tenant"] = await tenantResolver.ResolveAsync(tenantId);
        await _next(context);
    }
}
