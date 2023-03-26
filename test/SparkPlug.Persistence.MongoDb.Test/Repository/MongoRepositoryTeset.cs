namespace SparkPlug.Persistence.MongoDb.Test.Context;

public class TestModel : BaseEntity<string>
{
    public string? Name { get; set; }
}

public class TestRepo : MongoRepository<string, TestModel>
{
    public TestRepo(IMongoDbContext context, ILogger<TestRepo> logger) : base(context, logger)
    {
    }
}

public class MongoRepositoryTest
{
    private readonly Mock<IRepository<string, TestModel>> _mockRepo;
    public MongoRepositoryTest()
    {
        _mockRepo = new Mock<IRepository<string, TestModel>>();
    }
    [Fact]
    public void Test_MongoRepository()
    {
        var mockContext = new Mock<IMongoDbContext>();
        var mockLogger = new Mock<ILogger<TestRepo>>();
        var repo = new TestRepo(mockContext.Object, mockLogger.Object);
        // var createResult = await repo.CreateAsync(new CommandRequest<TestModel>(new TestModel() { Name = "Test" }));

        Assert.NotNull(repo);
        // Assert.NotNull(createResult);
        // Assert.Equal("Test", createResult.Name);
    }
}