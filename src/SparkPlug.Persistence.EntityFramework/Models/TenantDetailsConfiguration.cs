namespace SparkPlug.Persistence.EntityFramework.Models;

public class TenantDetailsConfiguration : IEntityTypeConfiguration<TenantDetails>
{
    public void Configure(EntityTypeBuilder<TenantDetails> builder)
    {
        builder.ToTable(nameof(TenantDetails));
        builder.HasKey(e => e.Id);
        builder.Property(e=>e.Name).HasMaxLength(64);
        builder.HasMany(p => p.Options);
    }
}