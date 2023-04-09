namespace SparkPlug.Persistence.EntityFramework.Context;

public interface ISqlDbModelConfiguration
{
    public void Configure(ModelBuilder modelBuilder);
}

public interface ITenantDbModelConfiguration : ISqlDbModelConfiguration { }
public interface IHomeDbModelConfiguration : ISqlDbModelConfiguration { }