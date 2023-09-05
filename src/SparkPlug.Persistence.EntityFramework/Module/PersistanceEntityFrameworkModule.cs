using Microsoft.AspNetCore.Builder;

namespace SparkPlug.Persistence.EntityFramework;

public class PersistanceEntityFrameworkModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SqlDbOptions>(configuration.GetSection(SqlDbOptions.ConfigPath));
        services.AddScoped<HomeDbContextOptions>();
        services.AddDbContext<HomeDbContext>(ServiceLifetime.Scoped);
        services.AddScoped(typeof(HomeRepository<,>));
        services.AddScoped(typeof(IOptions<DbConfig>), typeof(HomeOptions<DbConfig>));

        services.AddScoped<TenantDbContextOptions>();
        services.AddDbContext<TenantDbContext>(ServiceLifetime.Scoped);
        services.AddScoped(typeof(TenantRepository<,>));

        services.AddScoped<IRepositoryProvider, SqlRepositoryProvider>();
        services.AddSingleton<ITenantDbModelConfiguration, TenantModelConfigurations>();
        services.AddSingleton<IHomeDbModelConfiguration, HomeModelConfigurations>();

        services.AddHealthChecks().AddCheck<SqlDbHealthCheck>("SqlDb", tags: new[] { "sqldb", "all" });
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
        
    }

    public void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        
    }
}