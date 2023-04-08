namespace SparkPlug.Persistence.EntityFramework.Context;

public class SqlDbContext : DbContext
{
    private readonly IModelConfigurationProvider _modelConfigurationProvider;
    public SqlDbContext(IServiceProvider serviceProvider, SqlDbContextOptions sqlOptions) : base(sqlOptions.Value)
    {
        _modelConfigurationProvider = serviceProvider.GetRequiredService<IModelConfigurationProvider>();
        // Move to Create Tenant Api  - When onboard new tenant, this will create the db schema
        // Database EnsureCreated
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _modelConfigurationProvider.Configure(modelBuilder);
    }
    public async Task<int> SaveChangesAsync<TId>(TId id, CancellationToken cancellationToken)
    {
        var entries = ChangeTracker.Entries()
                  .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified) && x.Entity is IAuditableEntity<TId>);
        var dateTime = DateTime.UtcNow;
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(IAuditableEntity<TId>.CreatedAt)).CurrentValue = dateTime;
                entry.Property(nameof(IAuditableEntity<TId>.CreatedBy)).CurrentValue = id;
            }
            entry.Property(nameof(IAuditableEntity<TId>.ModifiedAt)).CurrentValue = dateTime;
            entry.Property(nameof(IAuditableEntity<TId>.ModifiedBy)).CurrentValue = id;
        }
        return await SaveChangesAsync(cancellationToken);
    }
}
