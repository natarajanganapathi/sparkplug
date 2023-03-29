namespace SparkPlug.Sample.Api;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddWebApi(Configuration);
        services.AddMongoDb(Configuration);
        services.AddScoped<ITenantResolver, TenantResolver>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(
                    (bearerOptions) => Configuration.Bind("SparkPlug:Api:AzureAd", bearerOptions),
                    (identityOptions) => Configuration.Bind("SparkPlug:Api:AzureAd", identityOptions));

        services.AddCors(options =>
        {
           options.AddDefaultPolicy(builder =>
           {
               builder.WithOrigins("http://localhost:*", "http://127.0.0.1:*")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
           });
       });
    }

    public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        app.UseCors();
        app.UseWebApi(serviceProvider);
        app.UseMongoDb(serviceProvider);
    }
}
