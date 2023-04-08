namespace SparkPlug.Api.Controllers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ApiAttribute : Attribute
{
    public ApiAttribute(string route)
    {
        Route = route;
    }
    public ApiAttribute(string route, Type genericControllerType) : this(route)
    {
        Type = genericControllerType;
    }
    public string Route { get; set; }
    public Type? Type { get; set; }
}
