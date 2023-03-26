namespace SparkPlug.Persistence.Abstractions;

public interface IDbContextOptionsProvider
{
    DbContextOptions GetDbContextOption(string connectionString);
}