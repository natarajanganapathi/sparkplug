namespace Microsoft.Extensions.DependencyInjection;

public static class ModuleLoader
{
    private static IEnumerable<IModule?> Modules = new List<IModule>();
    private static bool ModulesLoaded;
    // static ModuleLoader()
    // {
    //      Modules = AssemblyCache.Types
    //             .Where(t => t.IsClass && typeof(IModule).IsAssignableFrom(t))
    //             .Select(t => Activator.CreateInstance(t) as IModule);
    // }

    public static void AddCustomModules(this IServiceCollection services, IConfiguration configuration)
    {
        if (!ModulesLoaded)
        {
            Modules = LoadAssembly(configuration);
        }
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
    public static IEnumerable<IModule?> LoadAssembly(IConfiguration configuration)
    {
        IEnumerable<IModule?> result = new List<IModule>();
        var modules = configuration.GetSection("Modules")
                                   .GetChildren()
                                   .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                                   .Select(x => x.Value ?? string.Empty)
                                   .ToArray();
        if (modules?.Length > 0)
        {
            var assemblies = LoadAssemblies(modules);
            ModulesLoaded = true;
            result = assemblies.SelectMany(a => a.GetTypes())
                      .Where(t => t.IsClass && typeof(IModule).IsAssignableFrom(t))
                      .Select(t => Activator.CreateInstance(t) as IModule);
        }
        return result;
    }

    public static Assembly LoadAssembly(string assemblyName)
    {
        string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        string assemblyPath = Path.Combine(appDirectory, assemblyName);
        if (!File.Exists(assemblyPath))
        {
            Console.WriteLine(new StringBuilder().Append("Assembly file ").Append(assemblyPath).Append(" not found"));
        }
        return Assembly.LoadFrom(assemblyPath);
    }

    public static IEnumerable<Assembly> LoadAssemblies(string[] assemblyNames)
    {
        return assemblyNames.Select(LoadAssembly);
    }
}