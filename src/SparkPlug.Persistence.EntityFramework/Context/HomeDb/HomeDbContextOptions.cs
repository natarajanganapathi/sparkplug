namespace SparkPlug.Persistence.EntityFramework.Context;

public class HomeDbContextOptions : SqlDbContextOptions
{
    public HomeDbContextOptions(IDbContextOptionsProvider dbContextOptionsProvider, IOptions<DbConfig> dbConfig)
        : base(dbContextOptionsProvider, dbConfig.Value.ConnectionString) { }
}