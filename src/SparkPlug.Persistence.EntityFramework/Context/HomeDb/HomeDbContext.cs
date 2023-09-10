namespace SparkPlug.Persistence.EntityFramework.Context;

public class HomeDbContext : SqlDbContext
{
    private readonly IHomeDbModelConfiguration _modelConfigProvider;
    public HomeDbContext(IHomeDbModelConfiguration modelConfigProvider, HomeDbContextOptions sqlOptions) : base(sqlOptions.Value)
    {
        _modelConfigProvider = modelConfigProvider;
    }
    protected override ISqlDbModelConfiguration ModelConfigProvider => _modelConfigProvider;
}