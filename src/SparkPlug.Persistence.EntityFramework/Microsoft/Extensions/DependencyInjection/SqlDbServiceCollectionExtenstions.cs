namespace Microsoft.Extensions.DependencyInjection;

public static class SqlDbServiceCollectionExtenstions
{
    public static void AddSqlDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SqlDbOptions>(configuration.GetSection(SqlDbOptions.ConfigPath));
        services.AddScoped<SqlDbContextOptions>();
        services.AddDbContext<SqlDbContext>(ServiceLifetime.Scoped);
        services.AddScoped<IRepositoryProvider, SqlRepositoryProvider>();
        services.AddScoped(typeof(SqlRepository<,>));
        services.AddHealthChecks().AddCheck<SqlDbHealthCheck>("SqlDb", tags: new[] { "sqldb", "all" });
        services.AddHealthChecks().AddCheck<MultiTenantHealthCheck>("MultiTenant", tags: new[] { "multitenant"});
    }
}
