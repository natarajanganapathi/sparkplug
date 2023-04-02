namespace SparkPlug.Api.Controllers;

public class GenericControllerRouteConvention : IControllerModelConvention
{
    private const string Path = "{tenant}/api/v1/";

    public void Apply(ControllerModel controller)
    {
        if (controller.ControllerType.IsGenericType)
        {
            var argType = controller.ControllerType
                                      .GetGenericArguments()
                                      .Select(gtyp => (gtyp, gtyp.GetCustomAttribute<ApiAttribute>()))
                                      .FirstOrDefault(x => x.Item2 != null);
            var controllerName = string.IsNullOrWhiteSpace(argType.Item2?.Route) ? argType.gtyp.Name : argType.Item2.Route;
            var sb = new StringBuilder().Append(Path).Append(controllerName);
            controller.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(sb.ToString()))
            });
            controller.ControllerName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(controllerName);
        }
    }
}
