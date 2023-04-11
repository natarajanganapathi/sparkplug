namespace SparkPlug.Sample.WebApi.Models;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable(nameof(Department));
        builder.HasKey(e => e.Id);
        builder.Property(e => e.DepartmnetName).HasMaxLength(50);
        builder.Property(e => e.Location).HasMaxLength(50);
        builder.Property(e => e.Revision);
        builder.Property(e => e.Status);
    }
}