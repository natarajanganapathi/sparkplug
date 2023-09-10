namespace SparkPlug.Persistence.EntityFramework.Context;

public class TenantDbMigrationConetxt : DbContext
{
    private readonly ITenantDbModelConfiguration _modelConfigProvider;
    public TenantDbMigrationConetxt(ITenantDbModelConfiguration modelConfigProvider, TenantDbContextOptions options) : base(options.Value)
    {
        _modelConfigProvider = modelConfigProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _modelConfigProvider.Configure(modelBuilder);
    }
}
