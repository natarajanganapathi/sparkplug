namespace SparkPlug.Persistence.EntityFramework;

public class TenantModelConfigurations : ModelConfigurations, ITenantDbModelConfiguration
{
    public void Configure(ModelBuilder modelBuilder)
    {
        Configure<TenantDbAttribute>(modelBuilder);
    }
}