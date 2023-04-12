namespace SparkPlug.Api.Tests.Controllers;

public class GenericControllerRouteConventionTests
{
    [Fact]
    public void Apply_AddsSelectorModel_WhenControllerTypeIsGenericType()
    {
        // Arrange
        var convention = new GenericControllerRouteConvention(new RouteAttribute("{tenant}"), false);
        var controllerModel = new ControllerModel(typeof(ApiController<long, User>).GetTypeInfo(), Array.Empty<object>());

        // Act
        convention.Apply(controllerModel);

        // Assert
        Assert.Single(controllerModel.Selectors);
        Assert.NotNull(controllerModel.Selectors[0].AttributeRouteModel);
        Assert.Equal("user", controllerModel.Selectors[0].AttributeRouteModel?.Template);
    }

    [Api("user"), TenantDbEntity]
    public class User : BaseEntity<long> { }
}
