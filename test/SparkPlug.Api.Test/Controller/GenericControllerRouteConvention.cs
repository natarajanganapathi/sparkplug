namespace SparkPlug.Api.Tests.Controllers;

public class GenericControllerRouteConventionTests
{
    [Fact]
    public void Apply_AddsSelectorModel_WhenControllerTypeIsGenericType()
    {
        // Arrange
        var convention = new GenericControllerRouteConvention();
        var controllerModel = new ControllerModel(typeof(TestController<,>).GetTypeInfo(), Array.Empty<object>());

        // Act
        convention.Apply(controllerModel);

        // Assert
        Assert.Single(controllerModel.Selectors);
        Assert.NotNull(controllerModel.Selectors[0].AttributeRouteModel);
        Assert.Equal("TModel", controllerModel.Selectors[0].AttributeRouteModel?.Template);
    }

    public class TestController<TId, TModel>
    {
        public IActionResult? Index() => null;
    }
}
