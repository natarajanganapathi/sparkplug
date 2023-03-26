namespace SparkPlug.Api.Middleware;

public class TenantResolverMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolverMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantResolver tenantResolver, IOptions<WebApiOptions> options)
    {
        // var tenantId = context.Request.Headers["TenantId"]; // Read form header
        // var tenantId = context.Request.Query["TenantId"]; // Read from QueryString
        // Get Tenent Id from Payload message.
        // string tenantId = null;
        // if (context.Request.Method == HttpMethod.Post.Method || context.Request.Method == HttpMethod.Put.Method)
        // {
        //     string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        //     dynamic data = JsonConvert.DeserializeObject(requestBody);
        //     tenantId = data?.TenantId;
        // }
        // if (!string.IsNullOrEmpty(tenantId))
        // {
        //     context.Items["TenantId"] = tenantId;
        // }

        var tenantId = context.GetRouteValue(WebApiConstants.Tenant)?.ToString();
        context.Items["Tenant"] = await tenantResolver.ResolveAsync(tenantId);
        var segments = context.Request.Path.Value?.Split('/');
        if (options.Value.IsMultiTenant && segments?.Length > 0)
        {
            context.Request.Path = $"/{string.Join("/", segments.Skip(0))}";
        }
        await _next(context);
    }
}
