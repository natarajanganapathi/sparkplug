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
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();
        // services.AddWebApi(Configuration);
        // services.AddSqlDb(Configuration);
        services.AddOptions<SqlDbOptions>().Configure((options) => options.GetConnection = (connectionString) => new NpgsqlConnection(connectionString));
        services.AddScoped<IDbContextOptionsProvider, DbContextOptionsProvider>();
        // services.AddScoped<ITenantResolver, TenantResolver>();

        // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //         .AddMicrosoftIdentityWebApi(
        //             (bearerOptions) => Configuration.Bind("SparkPlug:Api:AzureAd", bearerOptions),
        //             (identityOptions) => Configuration.Bind("SparkPlug:Api:AzureAd", identityOptions));

        // builder.Services.AddAuthentication().AddMicrosoftAccount((options) =>
        //     {
        //         options.ClientId = "";
        //         options.ClientSecret = "";
        //     });

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

    public static void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        app.UseCors();
        // app.UseWebApi(serviceProvider);
    }
}
