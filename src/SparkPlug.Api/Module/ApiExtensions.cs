namespace SparkPlug.Api;

public static class ApiExtensions
{
    public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
    {
        opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
    }
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
    public static void UseHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Tags.Contains("all"),
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = Constants.JsonContentType;
                var result = JsonConvert.SerializeObject(report);
                await context.Response.WriteAsync(result);
            }
        });
        app.UseHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
    }
    public static IApplicationBuilder UseTransactionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TransactionMiddleware>();
    }
}
