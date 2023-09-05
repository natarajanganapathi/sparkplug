namespace SparkPlug.Api;

public class ApiModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddOptions();
        services.Configure<WebApiOptions>(configuration.GetSection(WebApiOptions.ConfigPath));
        services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
            options.Filters.Add<ApiExceptionFilterAttribute>();
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });
        services.AddHttpClient();
    }

    public void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider)
    {

    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }
}