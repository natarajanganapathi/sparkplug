namespace SparkPlug.Persistence.EntityFramework.PostgreSql;

public class DbContextOptionsProvider : IDbContextOptionsProvider
{
    public DbContextOptions GetDbContextOption(string connectionString)
    {
        return new DbContextOptionsBuilder()
                    .UseNpgsql(connectionString)
                    .Options;
    }
}