namespace SparkPlug.Api.Controllers;

public class GenericControllerRouteConvention : IControllerModelConvention
{
    private static readonly Type apiAttributeType = typeof(ApiAttribute);
    private readonly AttributeRouteModel? _tenantPrefix;
    public GenericControllerRouteConvention(IRouteTemplateProvider? routeTemplateProvider)
    {
        _tenantPrefix = routeTemplateProvider == null ? null : new AttributeRouteModel(routeTemplateProvider);
    }
    public void Apply(ControllerModel controller)
    {
        if (controller.ControllerType.IsGenericType)
        {
            var genericArguments = controller.ControllerType.GetGenericArguments();
            var type = genericArguments?.FirstOrDefault(x => x.IsDefined(apiAttributeType, inherit: false));
            var apiAttribute = type?.GetCustomAttribute<ApiAttribute>();
            if (apiAttribute != null)
            {
                var route = new AttributeRouteModel(new RouteAttribute(apiAttribute.Route));
                if (type?.GetCustomAttribute<TenantDbAttribute>() != null)
                {
                    route = AttributeRouteModel.CombineAttributeRouteModel(_tenantPrefix, route);
                }
                controller.Selectors.Add(new SelectorModel { AttributeRouteModel = route });
                controller.ControllerName = type!.Name;
            }
        }
    }
}
