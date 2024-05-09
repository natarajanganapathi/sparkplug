namespace SparkPlug.Hosts;

public class Startup
{
    private IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAllModules(Configuration);
        // services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        // .AddCookie(options =>
        // {
        //     options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        //     options.SlidingExpiration = true;
        // });
        services.AddDistributedMemoryCache();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi((bearerOptions) =>
                    {
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
        app.UseAllModules();
        // app.UseAuthentication();
        // app.UseAuthorization();
        // app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
