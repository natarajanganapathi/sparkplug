namespace SparkPlug.DesignTimeMigration.Context;

public class HomeDbMigrationContext : DbContext
{
    public HomeDbMigrationContext(DbContextOptions<HomeDbMigrationContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new HomeModelConfigurations().Configure(modelBuilder);
    }
}