namespace SparkPlug.Persistence.EntityFramework.Context;

public abstract class SqlDbContextOptions
{
    public DbContextOptions Value { get; }
    protected SqlDbContextOptions(IDbContextOptionsProvider dbContextOptionsProvider, string connectionString)
    {
        Value = dbContextOptionsProvider.GetDbContextOption(connectionString);
    }
}
