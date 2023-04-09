namespace SparkPlug.Persistence.EntityFramework;

public class HomeModelConfigurations : IHomeDbModelConfiguration
{
    public void Configure(ModelBuilder modelBuilder)
    {
        var homeModelsType = AssemblyCache.Assemblies.SelectMany(x => x.GetTypes())
             .Where(t => !string.IsNullOrEmpty(t.Namespace) && t.GetCustomAttributes<HomeDbEntityAttribute>().Any());
        foreach (var type in homeModelsType)
        {
            modelBuilder.ApplyConfiguration(Activator.CreateInstance(type) as dynamic);
        }
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantModelConfiguration).Assembly);
    }
}