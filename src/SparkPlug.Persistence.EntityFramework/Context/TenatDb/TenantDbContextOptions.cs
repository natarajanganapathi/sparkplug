namespace SparkPlug.Persistence.EntityFramework.Context;

public class TenantDbContextOptions : SqlDbContextOptions
{
    public TenantDbContextOptions(IDbContextOptionsProvider dbContextOptionsProvider, ITenantOptions<DbConfig> dbConfig)
        : base(dbContextOptionsProvider, dbConfig.Value.ConnectionString) { }
}
