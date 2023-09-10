namespace SparkPlug.Persistence.EntityFramework.Context;

public class HomeDbMigrationContext : DbContext
{
    private readonly IHomeDbModelConfiguration _modelConfigProvider;
    public HomeDbMigrationContext(IHomeDbModelConfiguration modelConfigProvider, HomeDbContextOptions options) : base(options.Value)
    {
        _modelConfigProvider = modelConfigProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _modelConfigProvider.Configure(modelBuilder);
    }
}
