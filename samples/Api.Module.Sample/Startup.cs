namespace Api.Module.Sample;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        // DynamicAssemblyLoader.LoadAssemblyFromConfiguration(Configuration);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddWebApi(Configuration);
        services.AddSqlDb(Configuration);
        services.AddOptions<SqlDbOptions>().Configure((options) => options.GetConnection = (connectionString) => new NpgsqlConnection(connectionString));
        services.AddScoped<IDbContextOptionsProvider, DbContextOptionsProvider>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(
                (bearerOptions) => {
                    Configuration.Bind("SparkPlug:Api:AzureAdB2C", bearerOptions);
                    bearerOptions.TokenValidationParameters.NameClaimType = "name";
                },
                (identityOptions) => Configuration.Bind("SparkPlug:Api:AzureAdB2C", identityOptions)
            );
        services.AddAuthorization(options => options.AddPolicy("CustomPolicy", policy => policy.RequireAuthenticatedUser()));

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
        app.UseWebApi(serviceProvider);
    }
}
