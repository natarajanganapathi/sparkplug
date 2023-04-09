namespace SparkPlug.Persistence.EntityFramework;

public class TenantModelConfigurations : ITenantDbModelConfiguration
{
    public void Configure(ModelBuilder modelBuilder)
    {
        var tenantModelsType = AssemblyCache.Assemblies.SelectMany(x => x.GetTypes())
             .Where(t => !string.IsNullOrEmpty(t.Namespace) && t.GetCustomAttributes<TenantDbEntityAttribute>().Any());
        foreach (var type in tenantModelsType)
        {
            modelBuilder.ApplyConfiguration(Activator.CreateInstance(type) as dynamic);
        }
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantModelConfiguration).Assembly);
    }
}