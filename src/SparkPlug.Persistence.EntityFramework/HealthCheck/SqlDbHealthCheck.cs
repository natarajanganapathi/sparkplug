namespace SparkPlug.Persistence.EntityFramework.HealthCheck;

public class SqlDbHealthCheck : IHealthCheck
{
    private readonly SqlDbOptions _options;

    public SqlDbHealthCheck(IOptions<SqlDbOptions> options)
    {
        _options = options.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var connection = _options.Connection ?? throw new ArgumentException("Connection is null");
            await _options.Connection.OpenAsync(cancellationToken);
            await _options.Connection.CloseAsync();
            return HealthCheckResult.Healthy();
        }
        catch(Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message, ex);
        }
    }
}