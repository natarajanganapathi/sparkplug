namespace Microsoft.Extensions.DependencyInjection;

public static class ApiServiceCollectionExtenstions
{
    public static IServiceCollection AddTenancyRouteAttribute(this IServiceCollection services)
    {
        services.AddScoped(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext?.Items["Tenant"] as ITenant ?? Tenant.Default);
        services.AddMvc(MvcOptions =>
        {
            IRouteTemplateProvider routeAttribute = new RouteAttribute("{tenant}");
            MvcOptions.UseCentralRoutePrefix(routeAttribute);
        });
        return services;
    }
}