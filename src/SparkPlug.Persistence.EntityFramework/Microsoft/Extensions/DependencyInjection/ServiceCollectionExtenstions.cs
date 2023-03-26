namespace Microsoft.Extensions.DependencyInjection;

public static class SqlServiceCollectionExtenstions
{
    public static void AddSqlDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SqlDbOptions>(configuration.GetSection(SqlDbOptions.ConfigPath));
        services.AddScoped<SqlDbContextOptions>();
        services.AddDbContext<SqlDbContext>(ServiceLifetime.Scoped);
        services.AddScoped<IRepositoryProvider, SqlRepositoryProvider>();
        services.AddScoped(typeof(SqlRepository<,>));
        services.AddHealthChecks().AddCheck<SqlDbHealthCheck>("SqlDb", tags: new[] { "sqldb", "all" });
    }

    public static void UseSqlDb(this IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        // app.MapGet("/db", ([FromServices] SparkPlugEntityFrameworkOptions options) => options);
    }
}
