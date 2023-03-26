namespace SparkPlug.Api.Controllers;

public class GenericControllerRouteConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (controller.ControllerType.IsGenericType)
        {
            var genericTypes = controller.ControllerType.GetGenericArguments();
            var model = genericTypes[1];
            var attribute = model.GetCustomAttribute<ApiAttribute>();
            var controllerName = string.IsNullOrWhiteSpace(attribute?.Route) ? model.Name : attribute.Route;
            controller.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(controllerName))
            });
            controller.ControllerName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(controllerName);
        }
    }
}
