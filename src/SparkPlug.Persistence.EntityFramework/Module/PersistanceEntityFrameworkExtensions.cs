namespace SparkPlug.Persistence.EntityFramework;

public static class PersistanceEntityFrameworkExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        
        RunDatabaseOperations<HomeDbMigrationContext>(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>());
        // RunDatabaseOperations<TenantDbContext>(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>());

        void RunDatabaseOperations<TContext>(IServiceScopeFactory scopeFactory) where TContext : DbContext
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
        }
    }
}
