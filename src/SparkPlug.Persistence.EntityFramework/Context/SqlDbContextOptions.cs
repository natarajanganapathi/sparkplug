namespace SparkPlug.Persistence.EntityFramework.Context;

public abstract class SqlDbContextOptions
{
    public DbContextOptions Value { get; }
    protected SqlDbContextOptions(IDbContextOptionsProvider dbContextOptionsProvider, string connectionString)
    {
        Value = dbContextOptionsProvider.GetDbContextOption(connectionString);
    }
}

public class TenantDbContextOptions : SqlDbContextOptions
{
    public TenantDbContextOptions(IDbContextOptionsProvider dbContextOptionsProvider, ITenantOptions<DbConfig> dbConfig)
        : base(dbContextOptionsProvider, dbConfig.Value.ConnectionString) { }
}

public class HomeDbContextOptions : SqlDbContextOptions
{
    public HomeDbContextOptions(IDbContextOptionsProvider dbContextOptionsProvider, IOptions<DbConfig> dbConfig)
        : base(dbContextOptionsProvider, dbConfig.Value.ConnectionString) { }
}