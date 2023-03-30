namespace SparkPlug.Contracts.Test.Request;

public class Test_CommmandRequest
{
    [Fact]
    public void Create_CommandRequest_With_No_Constructor_Perameter()
    {
        var cr = new CommandRequest<int>();
        Assert.NotNull(cr);
        Assert.Equal(0, cr.Data);
    }
    [Fact]
    public void Create_CommandRequest_With_Constructor_Perameter()
    {
        var cr = new CommandRequest<int>(100);
        Assert.NotNull(cr);
        Assert.Equal(100, cr.Data);
    }
}