namespace SparkPlug.Persistence.EntityFramework.Context;

public interface IModelConfigurationProvider
{
    public void Configure(ModelBuilder modelBuilder);
}
