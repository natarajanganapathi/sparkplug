namespace SparkPlug.Persistence.Abstractions;

public class PersistanceAbstractionModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRequestContext<>), typeof(RequestContext<>));
        services.AddScoped(typeof(BaseService<,>));
        services.AddScoped(typeof(ITenantOptions<>), typeof(TenantOptions<>));
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }

    public void UseModule(IApplicationBuilder app)
    {
    }
}