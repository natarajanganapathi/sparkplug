namespace SparkPlug.Persistence.EntityFramework.PostgreSql;

public class EntityFrameworkPostgreSqlModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkPostgreSql(configuration);
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {

    }

    public void UseModule(IApplicationBuilder app)
    {

    }
}