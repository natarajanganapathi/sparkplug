namespace SparkPlug.Hosts;

public static class ModuleLoader
{
    private static readonly IEnumerable<IModule?> Modules = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty, "*.dll")
            .Select(Assembly.LoadFrom)
            .SelectMany(x => x.GetTypes())
            .Where(t => t.IsClass && typeof(IModule).IsAssignableFrom(t))
            .Select(t => Activator.CreateInstance(t) as IModule);

    public static void AddAllModules(this IServiceCollection services, IConfiguration configuration)
    {
        Console.WriteLine($"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}: Total Modules (Add): {Modules.Count()}");
        foreach (var module in Modules)
        {
            module?.AddModule(services, configuration);
        }
    }
    public static void UseAllModules(this IApplicationBuilder app)
    {
        var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();
        logger.LogInformation($"Total Modules (Use): {string.Join(",", Modules.Select(x => x?.GetType().Name))}");
        app.UseGlobalExceptionHandling();
        app.UseHttpsRedirection();
        app.UseHealthChecks();
        app.UseRouting();
        app.UseWhen(context => context.Request.Method != "GET", appBuilder => appBuilder.UseTransactionMiddleware()); // Transaction not required in GET Method

        foreach (var module in Modules)
        {
            module?.UseModule(app);
        }
        app.UseAllMiddelwares();

        app.UseEndpoints(endpoints => endpoints.MapGet("/", async context => await context.Response.WriteAsync("Running!...")));
    }

    public static void UseAllMiddelwares(this IApplicationBuilder app)
    {
        foreach (var module in Modules)
        {
            module?.UseMiddelwares(app);
        }
    }
}