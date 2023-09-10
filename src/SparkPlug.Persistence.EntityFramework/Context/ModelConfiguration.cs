namespace SparkPlug.Persistence.EntityFramework;

public abstract class ModelConfigurations
{
    public void Configure<T>(ModelBuilder modelBuilder) where T : Attribute
    {
        var modelsType = AssemblyCache.Types
           .Where(t => !string.IsNullOrEmpty(t.Namespace) && t.GetCustomAttributes<T>().Any());
        foreach (var hmType in modelsType)
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