namespace Microsoft.Extensions.DependencyInjection;

public static class HealthCheckServiceCollectionExtensions
{
    public static void UseHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Tags.Contains("all"),
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = WebApiConstants.ContentType;
                var result = JsonConvert.SerializeObject(new
                {
                    status = report.Status.ToString(),
                    errors = report.Entries.Select(e => e.Value.Exception?.Message)
                });
                await context.Response.WriteAsync(result);
            }
        });
        app.UseHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
    }
}
