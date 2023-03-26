namespace SparkPlug.Contracts;

public abstract class ApiRequest : IApiRequest
{
    public string[]? Deps { get; set; }
}

public static partial class Extensions
{
    #region Depends
    public static IApiRequest AddDependency(this IApiRequest request, params string[] deps)
    {
        request.Deps = request.Deps?.Concat(deps).ToArray() ?? deps;
        return request;
    }
    #endregion
}
