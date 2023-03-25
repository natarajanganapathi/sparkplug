namespace SparkPlug.Sample.Api;

public class ModelConfigurationProvider : IModelConfigurationProvider
{
    public void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModelConfigurationProvider).Assembly);
    }
}