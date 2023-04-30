namespace SparkPlug.Menu.Repository.Sql;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable(nameof(MenuItem));
        builder.HasKey(e => e.Id);
    }
}