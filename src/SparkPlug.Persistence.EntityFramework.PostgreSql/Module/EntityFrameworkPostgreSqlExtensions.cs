namespace SparkPlug.Persistence.EntityFramework.PostgreSql;

public static class EntityFrameworkPostgreSqlExtensions
{
    public static IServiceCollection AddEntityFrameworkPostgreSql(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbContextOptionsProvider, DbContextOptionsProvider>();
        return services;
    }
}