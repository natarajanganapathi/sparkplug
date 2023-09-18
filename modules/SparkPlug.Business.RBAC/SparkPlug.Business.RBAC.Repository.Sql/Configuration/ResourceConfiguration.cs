namespace SparkPlug.Business.RBAC.Repository.Sql;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.ToTable(nameof(Resource));
        builder.HasKey(e => e.Id);
    }
}