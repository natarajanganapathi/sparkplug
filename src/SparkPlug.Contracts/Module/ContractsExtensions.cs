namespace SparkPlug.Contracts;

public static class ContractsExtensions
{
    public static IServiceCollection AddContracts(this IServiceCollection service)
    {
        ArgumentNullException.ThrowIfNull(service);
        service.AddSingleton<ISerializer, NewtonsoftSerializer>();
        return service;
    }
}