namespace Microsoft.Extensions.DependencyInjection;

public static class TenantResolverMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantResolverMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantResolverMiddleware>();
    }
}
