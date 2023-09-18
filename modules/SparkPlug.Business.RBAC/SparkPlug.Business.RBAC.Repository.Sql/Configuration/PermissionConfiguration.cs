namespace SparkPlug.Business.RBAC.Repository.Sql;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(nameof(Permission));
        builder.HasKey(e => e.Id);
    }
}