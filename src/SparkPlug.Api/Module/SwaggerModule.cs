
namespace SparkPlug.Api;

public class SwaggerModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwagger();
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }

    public void UseModule(IApplicationBuilder app)
    {
        var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        if (env.IsDevelopment())
        {
            app.UseSwaggerApi();
        }
    }
}