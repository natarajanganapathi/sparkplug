using SparkPlug.Api.Configuration;

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
        // services.AddOptions<ApiOptions>().Configure((options) => { });
        // services.AddSqlDb(Configuration);
        // services.AddOptions<SqlOptions>().Configure((options) => options.Connection = new NpgsqlConnection(options.ConnectionString));
        services.AddSqlDb(Configuration, (sqlOptions) =>
        {
            sqlOptions.Connection = new NpgsqlConnection(sqlOptions.ConnectionString);
            return sqlOptions;
        });
        services.AddScoped<IDbContextOptionsProvider, DbContextOptionsProvider>();
        services.AddSingleton<IModelConfigurationProvider, ModelConfigurationProvider>();
        services.AddScoped<ITenantResolver, TenantResolver>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi((bearerOptions) =>
            {
                Configuration.Bind("SparkPlug:Api:AzureAd", bearerOptions);
                // bearerOptions.TokenValidationParameters.NameClaimType = "name";
            }, (identityOptions) => Configuration.Bind("SparkPlug:Api:AzureAd", identityOptions));
        // builder.Services.AddAuthentication().AddMicrosoftAccount((options) =>
        //     {
        //         options.ClientId = "";
        //         options.ClientSecret = "";
        //     });
        services.AddCors(options =>
       {
           options.AddDefaultPolicy(builder =>
           {
               builder.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
           });
       });
    }

    public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        app.UseCors();
        app.UseWebApi(serviceProvider);
        app.UseSqlDb(serviceProvider);
    }
}
