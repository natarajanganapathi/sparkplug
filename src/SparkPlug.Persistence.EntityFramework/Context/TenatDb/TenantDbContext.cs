namespace SparkPlug.Persistence.EntityFramework.Context;

public class TenantDbContext : SqlDbContext
{
    private readonly ITenantDbModelConfiguration _modelConfigProvider;
    public TenantDbContext(ITenantDbModelConfiguration modelConfigProvider, TenantDbContextOptions sqlOptions) : base(sqlOptions.Value)
    {
        _modelConfigProvider = modelConfigProvider;
    }
    protected override ISqlDbModelConfiguration ModelConfigProvider => _modelConfigProvider;
}

