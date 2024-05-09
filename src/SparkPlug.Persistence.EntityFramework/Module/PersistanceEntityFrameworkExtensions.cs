namespace SparkPlug.Persistence.EntityFramework;

public static class PersistanceEntityFrameworkExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = GetServiceScope(app);
        RunDatabaseMigration<HomeDbContext>(scope);
        bool isSingleTenant = scope.ServiceProvider.GetRequiredService<ITenant>() is SingleTenant;
        if (isSingleTenant)
        {
            RunDatabaseMigration<TenantDbContext>(scope);
        }
    }
    private static IServiceScope GetServiceScope(IApplicationBuilder app)
    {
        var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
        return scopeFactory.CreateScope();
    }
    private static void RunDatabaseMigration<TContext>(IServiceScope scope) where TContext : DbContext
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        dbContext.Database.Migrate();
    }

    public static IServiceCollection AddPersistanceEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SqlDbOptions>(configuration.GetSection(SqlDbOptions.ConfigPath));
        services.AddOptions<SqlDbOptions>();
        services.AddScoped(typeof(ITenant), typeof(SingleTenant));

        services.AddDbContext<HomeDbContext>(ServiceLifetime.Scoped);
        services.AddScoped<HomeDbContextOptions>();
        services.AddScoped(typeof(HomeRepository<,>));
        services.AddScoped(typeof(IOptions<DbConfig>), typeof(HomeOptions<DbConfig>));

        services.AddDbContext<TenantDbContext>(ServiceLifetime.Scoped);
        services.AddScoped<TenantDbContextOptions>();
        services.AddScoped<TenantDbMigrationContext>();
        services.AddScoped(typeof(TenantRepository<,>));

        services.AddScoped<IRepositoryProvider, SqlRepositoryProvider>();
        services.AddSingleton<ITenantDbModelConfiguration, TenantModelConfigurations>();
        services.AddSingleton<IHomeDbModelConfiguration, HomeModelConfigurations>();

        services.AddHealthChecks().AddCheck<SqlDbHealthCheck>("SqlDb", tags: ["sqldb", "all"]);
        return services;
    }
}
