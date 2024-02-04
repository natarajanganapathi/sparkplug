namespace SparkPlug.Persistence.EntityFramework.Context;

public abstract class SqlDbContext : DbContext
{
    protected abstract ISqlDbModelConfiguration ModelConfigProvider { get; }
    protected SqlDbContext(DbContextOptions sqlOptions) : base(sqlOptions) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelConfigProvider.Configure(modelBuilder);
    }
    public async Task<int> SaveChangesAsync<TId>(TId userId, CancellationToken cancellationToken)
    {
        var entries = ChangeTracker.Entries()
                  .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified) && x.Entity is IAuditableEntity<TId>);
        var dateTime = DateTime.UtcNow;
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(IAuditableEntity<TId>.CreatedAt)).CurrentValue = dateTime;
                entry.Property(nameof(IAuditableEntity<TId>.CreatedBy)).CurrentValue = userId;
            }
            entry.Property(nameof(IAuditableEntity<TId>.ModifiedAt)).CurrentValue = dateTime;
            entry.Property(nameof(IAuditableEntity<TId>.ModifiedBy)).CurrentValue = userId;
        }
        return await SaveChangesAsync(cancellationToken);
    }
}
