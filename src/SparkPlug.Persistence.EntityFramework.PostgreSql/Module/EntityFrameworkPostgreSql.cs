namespace SparkPlug.Persistence.EntityFramework.PostgreSql;

public class EntityFrameworkPostgreSql : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
       services.AddScoped<IDbContextOptionsProvider, DbContextOptionsProvider>();
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
        
    }

    public void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        
    }
}