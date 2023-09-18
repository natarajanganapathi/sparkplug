namespace SparkPlug.DesignTimeMigration.Context;

public class HomeDbContextFactory : IDesignTimeDbContextFactory<HomeDbMigrationContext>
{
    public HomeDbMigrationContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<HomeDbMigrationContext>();
        var options = builder.UseNpgsql("").Options;
         return new HomeDbMigrationContext(options);
    }
}
