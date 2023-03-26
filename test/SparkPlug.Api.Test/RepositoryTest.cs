namespace SparkPlug.Api.Test;

public class Test_Repository
{
    [Fact]
    public async void Test_ListAsync()
    {
        var repo = GetRepository();
        var list = await repo.ListAsync(new QueryRequest());
        Assert.Single(list);
    }

    [Fact]
    public async void Test_GetAsync()
    {
        var repo = GetRepository();
        var list = await repo.GetAsync("63d2b49ef85fe3fe760b876d");
        var item = list.FirstOrDefault();
        Assert.Equal(ObjectId.Parse("63d2b49ef85fe3fe760b876d"), item?.Id);
    }

    private static PersonRepository GetRepository()
    {
        var config = new SparkPlugMongoDbOptions();
        GetConfiguration().Bind(SparkPlugMongoDbOptions.ConfigPath, config);

        // var config = GetConfiguration().GetSection(SparkPlugMongoDbOptions.);
        IMongoDbContext contxt = new MongoDbContext(config);
        var lf = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = lf.CreateLogger<PersonRepository>();
        return new PersonRepository(contxt, logger);
    }

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .AddEnvironmentVariables()
           .Build();
    }
}

public class PersonRepository : MongoRepository<ObjectId, Person>
{
    public PersonRepository(IMongoDbContext context, ILogger<PersonRepository> logger) : base(context, logger) { }
}

[SparkPlug.Persistence.MongoDb.Attributes.CollectionAttribute("Persons")]
public class Person : BaseEntity<ObjectId>
{
    public string? PersonName { get; set; }
    public string? Department { get; set; }
    public long Salary { get; set; }
    public Address? Address { get; set; }
    public string? MobileNo { get; set; }

    public bool Equals(Person other)
    {
        throw new System.NotImplementedException();
    }
}

public class Address
{
    public string? FlatNo { get; set; }
    public string? Street { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Pin { get; set; }
}


