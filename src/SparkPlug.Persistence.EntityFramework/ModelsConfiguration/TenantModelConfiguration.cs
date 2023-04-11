namespace SparkPlug.Persistence.EntityFramework;

public class TenantModelConfigurations : ITenantDbModelConfiguration
{
    public void Configure(ModelBuilder modelBuilder)
    {
        var tenantModelsType = AssemblyCache.Assemblies.SelectMany(x => x.GetTypes())
             .Where(t => !string.IsNullOrEmpty(t.Namespace) && t.GetCustomAttributes<TenantDbEntityAttribute>().Any());
        foreach (var tmType in tenantModelsType)
        {
            Type? configType = Array.Find(AssemblyCache.EntityTypeConfiguration, type =>
            {
                var etype = Array.Find(type.GetInterfaces(),
                        x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                    )?.GetGenericArguments().FirstOrDefault();
                return etype == tmType;
            });
            if (configType != null)
            {
                modelBuilder.ApplyConfiguration(Activator.CreateInstance(configType) as dynamic);
            }
        }
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantModelConfigurations).Assembly);
    }
}