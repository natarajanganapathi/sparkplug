namespace SparkPlug.Persistence.EntityFramework.Context;

public class SqlDbContextOptions
{
    public DbContextOptions Value { get; }
    public SqlDbContextOptions(IServiceProvider serviceProvider)
    {
        var dbContextOptionsProvider = serviceProvider.GetRequiredService<IDbContextOptionsProvider>();
        var tenantConfig = serviceProvider.GetRequiredService<ITenantOptions<TenantConfig>>();
        Value = dbContextOptionsProvider.GetDbContextOption(tenantConfig.Value.ConnectionString);
    }
}
