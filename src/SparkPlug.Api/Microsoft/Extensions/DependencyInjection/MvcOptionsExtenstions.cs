using Microsoft.AspNetCore.Mvc.Routing;

namespace Microsoft.Extensions.DependencyInjection;

public static class MvcOptionsExtensions
{
    public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
    {
        opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
    }
}