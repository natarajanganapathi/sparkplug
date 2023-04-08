namespace SparkPlug.Sample.Api;

public class ModelConfigurationProvider : IModelConfigurationProvider
{
    public void Configure(ModelBuilder modelBuilder)
    {
        var Assembly = typeof(ModelConfigurationProvider).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly);
    }
}