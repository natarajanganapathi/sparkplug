namespace SparkPlug.Persistence.EntityFramework;

public static class PersistanceEntityFrameworkExtensions
{
    public static void ApplyMigrationsForHomeDb(this IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        RunDatabaseOperations<HomeDbMigrationContext>(serviceProvider.GetRequiredService<IServiceScopeFactory>());
        bool isSingleTenant = serviceProvider.GetRequiredService<ITenant>() is SingleTenant;
        if (isSingleTenant)
        {
            RunDatabaseOperations<TenantDbMigrationContext>(serviceProvider.GetRequiredService<IServiceScopeFactory>());
        }
        void RunDatabaseOperations<TContext>(IServiceScopeFactory scopeFactory) where TContext : DbContext
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
            dbContext.Database.Migrate();
        }
    }
}
