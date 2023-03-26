namespace SparkPlug.Contracts.Test.Common;

public class User
{
    public string Name { get; set; } = string.Empty;
}

public class Test_CommandResponse
{
    [Fact]
    public void Create_CommandResponse_With_Constructor_Perameter()
    {
        var qr = new CommandResponse(new User() { Name = "Demo" });
        Assert.NotNull(qr);
        Assert.Equal("Demo", qr.Data.Value<string>(nameof(User.Name)));
        Assert.Null(qr.Message);
    }

    [Fact]
    public void Create_CommandResponse_With_No_Constructor_Perameter()
    {
        var qr = new CommandResponse();
        Assert.NotNull(qr);
        Assert.Null(qr.Data.Value<string>(nameof(User.Name)));
        Assert.Null(qr.Message);
    }
}