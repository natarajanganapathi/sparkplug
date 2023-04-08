namespace SparkPlug.Api.Tests.Controllers;

public class GenericControllerRouteConventionTests
{
    [Fact]
    public void Apply_AddsSelectorModel_WhenControllerTypeIsGenericType()
    {
        // Arrange
        var convention = new GenericControllerRouteConvention(new RouteAttribute("{tenant}"), false);
        var controllerModel = new ControllerModel(typeof(TestController).GetTypeInfo(), Array.Empty<object>());

        // Act
        convention.Apply(controllerModel);

        // Assert
        Assert.Single(controllerModel.Selectors);
        Assert.NotNull(controllerModel.Selectors[0].AttributeRouteModel);
        Assert.Equal("TModel", controllerModel.Selectors[0].AttributeRouteModel?.Template);
    }

    [ApiController, Route("test"), ApiExplorerSettings(GroupName = "v1")]
    public class TestController : BaseController<long, User>
    {
        public TestController(IServiceProvider serviceProvider) : base(serviceProvider) { }
        public IActionResult? Index() => null;
    }

    [Api("user")]
    public class User : BaseEntity<long> { }
}
