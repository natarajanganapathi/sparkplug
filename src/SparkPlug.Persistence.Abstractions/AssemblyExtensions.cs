namespace SparkPlug;

public static class AssemblyCache
{
    public static Assembly[] Assemblies { get; } = AppDomain.CurrentDomain.GetAssemblies();
    public static IEnumerable<Type> Types { get; } = Assemblies.SelectMany(x => x.GetTypes());
    public static Type[] EntityTypeConfiguration { get; } = Types.Where(type => !type.IsAbstract && type.GetInterfaces()
            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))).ToArray();
}