using Microsoft.AspNetCore.Http;
using SparkPlug.Api.Configuration;

namespace SparkPlug.Api.Middleware.Tests;
public class GlobalExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldCallNext_WhenNoExceptionIsThrown()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
        var nextMock = new Mock<RequestDelegate>();
        var middleware = new GlobalExceptionHandlingMiddleware(loggerMock.Object, nextMock.Object);
        var context = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextMock.Verify(next => next(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleExceptionAndReturnInternalServerErrorResponse_WhenExceptionIsThrown()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
        var nextMock = new Mock<RequestDelegate>();
        var middleware = new GlobalExceptionHandlingMiddleware(loggerMock.Object, nextMock.Object);
        var context = new DefaultHttpContext();
        var exception = new Exception("Test exception message");
        nextMock.Setup(next => next(context)).ThrowsAsync(exception);
        var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Equal(Constants.JsonContentType, context.Response.ContentType);
        responseBodyStream.Seek(0, SeekOrigin.Begin);
        var responseContent = await new StreamReader(responseBodyStream).ReadToEndAsync();
        // Assert.Contains("Internal Server Error", responseContent);
        Assert.Contains(exception.Message, responseContent);
        // loggerMock.Verify(logger => logger.LogError(It.Is<string>(msg => msg.Contains(exception.Message)), exception), Times.Once);
    }
}
