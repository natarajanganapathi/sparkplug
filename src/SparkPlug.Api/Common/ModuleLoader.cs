namespace SparkPlug.Api.Common;

public static class ModuleLoader
{
    private static readonly IEnumerable<IModule?> Modules;
    static ModuleLoader()
    {
        string exeDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? ".";
        Modules = Directory.GetFiles(exeDirectory, "*.dll")
                        .Select(Assembly.LoadFrom)
                        .SelectMany(x => x.GetTypes())
                        .Where(t => t.IsClass && typeof(IModule).IsAssignableFrom(t))
                        .Select(t => Activator.CreateInstance(t) as IModule);
    }

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