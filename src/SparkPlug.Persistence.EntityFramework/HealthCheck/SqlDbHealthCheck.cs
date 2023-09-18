namespace SparkPlug.Persistence.EntityFramework.HealthCheck;

public class SqlDbHealthCheck : IHealthCheck
{
    private readonly HomeDbContext _context;

    public SqlDbHealthCheck(HomeDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
            return await _context.Database.CanConnectAsync(cancellationToken) 
            ? HealthCheckResult.Healthy() 
            : HealthCheckResult.Unhealthy("Not able to connect database.");
    }
}
