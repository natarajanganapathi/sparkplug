namespace SparkPlug.Api.Controllers;

public class GenericTypeControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    private readonly Type _controllerType;

    public GenericTypeControllerFeatureProvider(Type controllerType)
    {
        _controllerType = controllerType;
    }
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var assemblies = SwaggerServiceCollectionExtensions.CachedAssemblies;
        var models = assemblies.SelectMany(x => x.GetExportedTypes().Where(x => x.GetCustomAttributes<ApiAttribute>().Any()));
        foreach (var model in models)
        {
            var baseType = model.BaseType;
            var genericTypes = baseType?.GetGenericArguments();
            if (model.BaseType == typeof(object) || genericTypes?.Length != 1)
            {
                throw new ArgumentException("Api attribute should be used only on IBaseModel<> implementation");
            }
            var idType = genericTypes[0];
            var type = model.GetCustomAttribute<ApiAttribute>()?.Type ?? _controllerType;
            var controller = type.MakeGenericType(idType, model);
            feature.Controllers.Add(controller.GetTypeInfo());
        }
    }
}
