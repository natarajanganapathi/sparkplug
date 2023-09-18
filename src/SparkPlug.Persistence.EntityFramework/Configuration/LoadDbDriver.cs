// namespace SparkPlug.Persistence.EntityFramework.Configuration;

// public class LoadDbDriver
// {
//     private ConstructorInfo? _constructorInfo;
//     public LoadDbDriver(SqlDbOptions options)
//     {
//         if (string.IsNullOrWhiteSpace(options.FullyQualifiedClassName))
//         {
//             throw new ArgumentException("FullyQualifiedClassName property is null");
//         }
//         var type = Type.GetType(options.FullyQualifiedClassName);
//         _constructorInfo = type?.GetConstructor(new[] { typeof(string) });

//     }
//     public DbConnection GetDbDriver(string? connectionString)
//     {
//         if (string.IsNullOrWhiteSpace(connectionString))
//         {
//             throw new ArgumentException("ConnectionString property is null");
//         }
//         DbConnection? dbConnection = _constructorInfo?.Invoke(new object[] { connectionString }) as DbConnection;
//         return dbConnection ?? throw new Exception("Not able to create DbConnection with given database configuration");
//     }
// }