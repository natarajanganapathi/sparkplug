namespace Microsoft.Extensions.DependencyInjection;

public static class ApiServiceCollectionExtenstions
{
    public static IServiceCollection AddTenancyRouteAttribute(this IServiceCollection services)
    {
        services.Where(x => x.ServiceType == typeof(ITenant)).All(services.Remove); // Remove existing all ITenant registration
        services.AddScoped(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext?.Items["Tenant"] as ITenant ?? Tenant.Default);
        services.AddMvc(MvcOptions =>
        {
            IRouteTemplateProvider routeAttribute = new RouteAttribute("{tenant}");
            MvcOptions.UseCentralRoutePrefix(routeAttribute);
        });
        return services;
    }
}