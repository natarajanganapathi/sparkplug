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
            if (_options.GetConnection == null) throw new ArgumentException("Connection is null");
            using var connection = _options.GetConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);
            await connection.CloseAsync();
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message, ex);
        }
    }
}