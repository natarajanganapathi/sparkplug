namespace SparkPlug.Persistence.EntityFramework.Configuration;

public class SqlDbOptions
{
    public const string ConfigPath = "SparkPlug:SqlDb";
    
    [Required]
    public string? ConnectionString { get; set; }
    // [Required]
    // public string? DbConnectionFullyQualifiedClassName { get; set; }
    // public string? DbContextOptionsBuilderFullyQualifiedClassName { get; set; }
    // public string? MethodName { get; set; }
    // private ConstructorInfo? _constructorInfo;
    // public ConstructorInfo ConstructorInfo
    // {
    //     get
    //     {
    //         if (_constructorInfo == null)
    //         {
    //             if (string.IsNullOrWhiteSpace(DbConnectionFullyQualifiedClassName))
    //             {
    //                 throw new ArgumentException("FullyQualifiedClassName property is null");
    //             }
    //             var type = Type.GetType(DbConnectionFullyQualifiedClassName);
    //             _constructorInfo = type?.GetConstructor(new[] { typeof(string) });
    //         }
    //         return _constructorInfo ?? throw new Exception("Not able to create DbConnection with given database configuration");
    //     }
    // }
    // public DbConnection GetConnection()
    // {
    //     if (string.IsNullOrWhiteSpace(ConnectionString))
    //     {
    //         throw new ArgumentException("ConnectionString property is null");
    //     }
    //     return GetConnection(ConnectionString);
    // }
    // public DbConnection GetConnection(string connectionString)
    // {
    //     DbConnection? dbConnection = _constructorInfo?.Invoke(new object[] { connectionString }) as DbConnection;
    //     return dbConnection ?? throw new Exception("Not able to create DbConnection with given database configuration");
    // }
    // public DbContextOptions GetDbContextOptions()
    // {
    //     if (string.IsNullOrWhiteSpace(ConnectionString))
    //     {
    //         throw new ArgumentException("ConnectionString property is null");
    //     }
    //     return GetDbContextOptions(ConnectionString);
    // }
    // public DbContextOptions GetDbContextOptions(string connectionString)
    // {
    //     if (string.IsNullOrWhiteSpace(DbContextOptionsBuilderFullyQualifiedClassName))
    //     {
    //         throw new ArgumentException("Property DbContextOptionsBuilderFullyQualifiedClassName should not be null");
    //     }
    //     if (string.IsNullOrWhiteSpace(MethodName))
    //     {
    //         throw new ArgumentException("Property MethodName should not be null");
    //     }
    //     var optionsBuilder = new DbContextOptionsBuilder();
    //     var methodInfo = Type.GetType(DbContextOptionsBuilderFullyQualifiedClassName)?
    //                               .GetMethod(MethodName, new[] { typeof(DbContextOptionsBuilder), typeof(string) });
    //     methodInfo?.Invoke(null, new object[] { optionsBuilder, connectionString });
    //     return optionsBuilder.Options;
    // }
}
