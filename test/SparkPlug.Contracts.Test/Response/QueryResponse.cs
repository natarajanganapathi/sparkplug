namespace SparkPlug.Contracts.Test.Common;

public class Test_QueryResponse
{
    [Fact]
    public void Create_QueryResponse_With_No_Constructor_Perameter()
    {
        var qr = new QueryResponse();
        Assert.NotNull(qr);
        Assert.Null(qr.Data);
        Assert.Null(qr.Message);
        Assert.Null(qr.Page);
        // Assert.Null(qr.Total);
    }

    [Fact]
    public void Create_QueryResponse_With_Constructor_Perameter()
    {
        var qr = new QueryResponse(new int[] { 100 }, new PageContext(1, 100));
        Assert.NotNull(qr);
        Assert.Null(qr.Code);
        Assert.Null(qr.Message);
        Assert.NotNull(qr.Data);
        Assert.Equal(1, qr.Page?.PageNo);
        Assert.Equal(100, qr.Page?.PageSize);
        // Assert.Equal(1000, qr.Total);
        Assert.Equal(100, qr.Data?.Value<int>(0));
    }

    // [Fact]
    // public void Create_QueryResponse_with_Data()
    // {
    //     var qr = new QueryResponse()
    //     .AddResponse(new int[] { 100 })
    //     .AddResponse(new int[] { 1000 })
    //     .AddResponse(new int[] { 10000 })
    //     .AddPageContext(new PageContext(1, 100));
    //     // .AddTotalRecord(1000);

    //     Assert.NotNull(qr);
    //     Assert.Null(qr.Code);
    //     Assert.Null(qr.Message);
    //     Assert.NotNull(qr.Data);
    //     Assert.Equal(1, qr.Page?.PageNo);
    //     Assert.Equal(100, qr.Page?.PageSize);
    //     // Assert.Equal(1000, qr.Total);
    //     Assert.Equal(100, qr.Data?.First());
    //     Assert.Equal(1000, qr.Data?[1]);
    //     Assert.Equal(10000, qr.Data?[2]);
    // }
}
