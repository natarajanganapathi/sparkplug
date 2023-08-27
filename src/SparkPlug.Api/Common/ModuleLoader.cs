namespace SparkPlug.Api.Common;

public static class ModuleLoader
{
    private static readonly IEnumerable<IModule?> Modules = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty, "*.dll")
            .Select(Assembly.LoadFrom)
            .SelectMany(x => x.GetTypes())
            .Where(t => t.IsClass && typeof(IModule).IsAssignableFrom(t))
            .Select(t => Activator.CreateInstance(t) as IModule);

    public static void AddCustomModules(this IServiceCollection services)
    {
        foreach (var module in Modules)
        {
            module?.AddModule(services);
        }
    }
    public static void UseCustomModules(this IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        foreach (var module in Modules)
        {
            module?.UseModule(app, serviceProvider);
        }
    }

    public static void UseCustomModulesMiddelwares(this IApplicationBuilder app)
    {
        foreach (var module in Modules)
        {
            module?.UseMiddelwares(app);
        }
    }
}