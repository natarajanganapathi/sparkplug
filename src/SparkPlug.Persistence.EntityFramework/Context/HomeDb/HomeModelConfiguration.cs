namespace SparkPlug.Persistence.EntityFramework;

public class HomeModelConfigurations : ModelConfigurations, IHomeDbModelConfiguration
{
    public void Configure(ModelBuilder modelBuilder)
    {
        Configure<HomeDbAttribute>(modelBuilder);
    }
}