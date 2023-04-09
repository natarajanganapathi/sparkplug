namespace SparkPlug.Sample.WebApi.Models;

[TenantDbEntity]
public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable(nameof(Address));
        builder.HasKey(e => e.Id);
        builder.Property(e => e.FlatNo).HasMaxLength(50);
        builder.Property(e => e.Street).HasMaxLength(50);
        builder.Property(e => e.State).HasMaxLength(50);
        builder.Property(e => e.Country).HasMaxLength(50);
        builder.Property(e => e.Pin).HasMaxLength(50);
    }
}