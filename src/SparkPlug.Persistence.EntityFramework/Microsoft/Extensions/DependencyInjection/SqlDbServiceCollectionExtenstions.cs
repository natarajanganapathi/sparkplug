namespace Microsoft.Extensions.DependencyInjection;

public static class SqlDbServiceCollectionExtenstions
{
    public static void AddSqlDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SqlDbOptions>(configuration.GetSection(SqlDbOptions.ConfigPath));

        services.AddScoped<HomeDbContextOptions>();
        services.AddDbContext<HomeDbContext>(ServiceLifetime.Scoped);
        services.AddScoped(typeof(HomeRepository<,>));
        services.AddScoped(typeof(IOptions<DbConfig>), typeof(HomeOptions<DbConfig>));

        services.AddScoped<TenantDbContextOptions>();
        services.AddDbContext<TenantDbContext>(ServiceLifetime.Scoped);
        services.AddScoped(typeof(TenantRepository<,>));
        services.AddScoped(typeof(ITenantOptions<>), typeof(TenantOptions<>));

        services.AddScoped<IRepositoryProvider, SqlRepositoryProvider>();
        services.AddSingleton<ITenantDbModelConfiguration, TenantModelConfigurations>();
        services.AddSingleton<IHomeDbModelConfiguration, HomeModelConfigurations>();

        services.AddHealthChecks().AddCheck<SqlDbHealthCheck>("SqlDb", tags: new[] { "sqldb", "all" });
        services.AddHealthChecks().AddCheck<MultiTenantHealthCheck>("MultiTenant", tags: new[] { "multitenant"});
    }
}
