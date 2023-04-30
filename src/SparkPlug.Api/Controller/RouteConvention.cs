namespace SparkPlug.Api.Controllers;

public class RouteConvention : IApplicationModelConvention
{
    private readonly AttributeRouteModel _centralPrefix;
    private static readonly Type baseEntityInterfaceType = typeof(IBaseEntity<>);
    public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
    {
        _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
    }
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            if (IsMultiTenant(controller.ControllerType))
            {
                foreach (var selector in controller.Selectors)
                {
                    if (selector.AttributeRouteModel != null)
                    {
                        selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix, selector.AttributeRouteModel);
                    }
                }
            }
        }
    }
    public static bool IsMultiTenant(Type? type)
    {
        return type?.BaseType?.IsGenericType == true
         && (type.BaseType.GenericTypeArguments.Any(x => x.IsClass
                    && x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == baseEntityInterfaceType)
                    && x.GetCustomAttribute<TenantDbAttribute>() != null)
                || IsMultiTenant(type.BaseType)
            );
    }
}
