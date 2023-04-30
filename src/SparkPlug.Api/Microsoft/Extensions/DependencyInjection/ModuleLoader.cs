namespace Microsoft.Extensions.DependencyInjection;

public static class ModuleLoader
{
    private static readonly IEnumerable<IModule?> Modules;
    static ModuleLoader()
    {
         Modules = AssemblyCache.Types
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

    public static void UseCustomModulesMiddelware(this IApplicationBuilder app)
    {
        foreach (var module in Modules)
        {
            module?.UseMiddelwares(app);
        }
    }
}