namespace SparkPlug.Persistence.EntityFramework;

public class PersistanceEntityFrameworkModule : IModule
{
  public void AddModule(IServiceCollection services, IConfiguration configuration)
  {
    services.AddPersistanceEntityFramework(configuration);
  }

  public void UseMiddelwares(IApplicationBuilder app)
  {
  }

  public void UseModule(IApplicationBuilder app)
  {
    app.ApplyMigrations();
  }
}