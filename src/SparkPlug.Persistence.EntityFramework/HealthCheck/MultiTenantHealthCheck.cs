namespace SparkPlug.Persistence.EntityFramework.HealthCheck;

public class MultiTenantHealthCheck : IHealthCheck
{
    private readonly IServiceProvider _serviceProvider;

    public MultiTenantHealthCheck(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var tenantResolver = _serviceProvider.GetRequiredService<TenantResolver>();
            var options = _serviceProvider.GetRequiredService<IOptions<SqlDbOptions>>().Value;
            var tenants = await tenantResolver.GetAllTenantsAsync() ?? throw new Exception("There is no tenants onboarded");
            var results = new Dictionary<string, HealthCheckResult>();

            if (options.GetConnection == null) throw new ArgumentException("GetConnection delegate is null");
            foreach (var tenant in tenants)
            {
                try
                {
                    var connectionString = tenant.Options.Find(x => x.Key.Contains(SqlDbOptions.ConfigPath))?.Value ?? throw new Exception(new StringBuilder().Append(tenant.Id).Append(" does not have connection string").ToString());
                    using var connection = options.GetConnection(connectionString);
                    await connection.OpenAsync(cancellationToken);
                    await connection.CloseAsync();
                    results.Add(tenant.Id.ToString(), HealthCheckResult.Healthy());
                }
                catch (Exception ex)
                {
                    results.Add(tenant.Id.ToString(), new HealthCheckResult(context.Registration.FailureStatus, exception: ex));
                }
            }
            var response = results.ToDictionary(x => x.Key, x => (object)x.Value);
            return results.Values.Any(r => r.Status != HealthStatus.Healthy)
            ? new HealthCheckResult(context.Registration.FailureStatus, data: response)
            : HealthCheckResult.Healthy("All tenants are healthy", data: response);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message, ex);
        }
    }
}