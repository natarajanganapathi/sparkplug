namespace SparkPlug.Sample.WebApi.Models;

[TenantDbEntity]
public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable(nameof(Person));
        builder.HasKey(e => e.Id);
        builder.Property(e => e.PersonName).HasMaxLength(50);
        builder.Property(e => e.Department).HasMaxLength(50);
        builder.Property(e => e.Salary);
        builder.Property(e => e.MobileNo);
        builder.Property(e => e.AddressId);
        builder.Property(e => e.Revision);
        builder.Property(e => e.Status);

        builder.HasOne(p => p.Address)
               .WithOne(a => a.Person).HasForeignKey<Person>(x => x.AddressId);
    }
}