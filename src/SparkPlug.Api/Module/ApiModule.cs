namespace SparkPlug.Api;

public class ApiModule : IModule
{
    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddHttpContextAccessor();
        services.AddOptions();
        services.Configure<WebApiOptions>(configuration.GetSection(WebApiOptions.ConfigPath));
        services.AddControllers(options =>
        {
            options.Filters.Add(new AuthorizeFilter(policy));
            options.Filters.Add<ApiExceptionFilterAttribute>();
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });
        // services.AddHttpClient();
        services.AddHealthChecks();
    }

    public void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        app.UseExceptionHandler();
    }

    public void UseMiddelwares(IApplicationBuilder app)
    {
    }
}