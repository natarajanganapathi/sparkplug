namespace SparkPlug;

public static class AssemblyCache
{
    public static Assembly[] Assemblies { get; } = AppDomain.CurrentDomain.GetAssemblies();
}