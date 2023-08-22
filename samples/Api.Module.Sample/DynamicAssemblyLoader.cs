// namespace Api.Module.Sample;

// public static class DynamicAssemblyLoader
// {
//     public static void LoadAssemblyFromConfiguration(IConfiguration configuration)
//     {
//         var modules = configuration.GetSection("Modules")
//                                    .GetChildren()
//                                    .Where(x => !string.IsNullOrWhiteSpace(x.Value))
//                                    .Select(x => x.Value ?? string.Empty)
//                                    .ToArray();
//         if (modules?.Length > 0)
//         {
//             LoadAssemblies(modules);
//         }
//     }

//     public static void LoadAssemblies(string[] assemblyNames)
//     {
//         _ = assemblyNames.Select(LoadAssembly).ToList();
//     }
//     public static Assembly LoadAssembly(string assemblyName)
//     {
//         string appDirectory = AppContext.BaseDirectory;
//         string assemblyPath = Path.Combine(appDirectory, assemblyName);
//         if (!File.Exists(assemblyPath))
//         {
//             Console.WriteLine(new StringBuilder().Append("Assembly file '").Append(assemblyPath).Append("' not found"));
//         }
//         return Assembly.LoadFrom(assemblyPath);
//     }
// }
