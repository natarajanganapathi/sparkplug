namespace SparkPlug.Tenancy.Repository.Sql;

public class TenantOptionConfiguration : IEntityTypeConfiguration<TenantOption>
{
    public void Configure(EntityTypeBuilder<TenantOption> builder)
    {
        builder.ToTable(nameof(TenantOption));
        builder.HasKey(e => e.Id);
    }
}