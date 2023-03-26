namespace Microsoft.Extensions.DependencyInjection;

public static class SwaggerServiceCollectionExtensions
{
    private static readonly string?[] _apiVersions = CachedAssemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsDefined(typeof(ApiExplorerSettingsAttribute), false))
            .SelectMany(type => type.GetCustomAttributes<ApiExplorerSettingsAttribute>())
            .Select(a => a.GroupName)
            .Distinct()
            .ToArray();

    private static Assembly[]? _assemblies;

    public static Assembly[] CachedAssemblies { get => _assemblies ??= AppDomain.CurrentDomain.GetAssemblies(); }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(swaggerOptions =>
            {
                Array.ForEach(_apiVersions, version => swaggerOptions.SwaggerDoc(version, new OpenApiInfo { Version = version, Title = "Api" }));
                swaggerOptions.DocumentFilter<SetBasePathFilter>();
                swaggerOptions.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                swaggerOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference{ Type=ReferenceType.SecurityScheme, Id="Bearer"}
                        },
                        Array.Empty<string>()
                    }
                });
            });
        return services;
    }

    public static void UseSwaggerApi(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(uiOptions => Array.ForEach(_apiVersions, version => uiOptions.SwaggerEndpoint($"/swagger/{version}/swagger.json", version)));
    }
}

public class SetBasePathFilter : IDocumentFilter
{
    private readonly WebApiOptions _options;
    public SetBasePathFilter(IOptions<WebApiOptions> options)
    {
        _options = options.Value;
    }
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = _options.PathBase }
            };
    }
}