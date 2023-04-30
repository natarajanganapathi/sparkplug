namespace SparkPlug.Persistence.EntityFramework;

public class HomeModelConfigurations : IHomeDbModelConfiguration
{
    public void Configure(ModelBuilder modelBuilder)
    {
        var homeModelsType = AssemblyCache.Types
            .Where(t => !string.IsNullOrEmpty(t.Namespace) && t.GetCustomAttributes<HomeDbAttribute>().Any());
        foreach (var hmType in homeModelsType)
        {
            Type? configType = Array.Find(AssemblyCache.EntityTypeConfiguration, type =>
            {
                var etype = Array.Find(type.GetInterfaces(),
                        x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                    )?.GetGenericArguments().FirstOrDefault();
                return etype == hmType;
            });
            if (configType != null)
            {
                modelBuilder.ApplyConfiguration(Activator.CreateInstance(configType) as dynamic);
            }
        }
    }
}