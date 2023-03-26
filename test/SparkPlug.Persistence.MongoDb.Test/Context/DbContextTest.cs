namespace SparkPlug.Persistence.MongoDb.Test.Context;

public class DbContextTest
{
    private readonly Mock<IMongoDatabase> _mockDB;
    private readonly Mock<IMongoClient> _mockClient;
    private readonly Mock<IMongoCollection<SparkPlugMongoDbOptions>> _mockCollection;
    public DbContextTest()
    {
        _mockDB = new Mock<IMongoDatabase>();
        _mockClient = new Mock<IMongoClient>();
        _mockCollection = new Mock<IMongoCollection<SparkPlugMongoDbOptions>>();
    }
    [Fact]
    public void Test_DbContext()
    {

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"SparkPlug:MongoDb:ConnectionString", "mongodb://localhost:27017/test"}
            })
            .Build();

        _mockClient
            .Setup(c => c.GetDatabase(It.IsAny<string>(), default))
            .Returns(_mockDB.Object);

        _mockDB.Setup(d => d.GetCollection<SparkPlugMongoDbOptions>(It.IsAny<string>(), default)).Returns(_mockCollection.Object);


        var mongoDbOption = new SparkPlugMongoDbOptions();
        config.Bind(SparkPlugMongoDbOptions.ConfigPath, mongoDbOption);
        var context = new MongoDbContext(mongoDbOption);
        Assert.NotNull(context);
        var collection = context.GetCollection<SparkPlugMongoDbOptions>("Test");

        //Act 
        // new ConfigurationBuilder().Build().Bind(SparkPlugMongoDbOptions.ConfigPath, mongoDbOption);
        // Assert.Throws<ArgumentException>(() => new MongoDbContext(mongoDbOption));
    }
}