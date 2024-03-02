using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SparkPlug.Business.RBAC.Service;

public class EndpointInfo
{
    public string? Namespace { get; set; }
    public string? Library { get; set; }
    public string? Route { get; set; }
    public string? Verb { get; set; }
}

public class EndpointService : BaseService<long, Resource>
{
    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

    public EndpointService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _actionDescriptorCollectionProvider = GetService<IActionDescriptorCollectionProvider>();
    }

    public IEnumerable<EndpointInfo?> GetAllEndpoints()
    {
        return _actionDescriptorCollectionProvider.ActionDescriptors.Items
            .Select(ExtractEndpointInfo);
    }

    private static EndpointInfo? ExtractEndpointInfo(ActionDescriptor actionDescriptor)
    {
        var match = GetMatch(actionDescriptor.DisplayName!);
        return match.Success
            ? new EndpointInfo
            {
                Namespace = match.Groups[nameof(EndpointInfo.Namespace)].Value,
                Library = match.Groups[nameof(EndpointInfo.Library)].Value,
                Route = actionDescriptor.AttributeRouteInfo?.Template,
                Verb = actionDescriptor.AttributeRouteInfo?.Name
            }
            : null;
    }

    private static Match GetMatch(string displayName)
    {
        const string pattern = @$"(?<{nameof(EndpointInfo.Namespace)}>[\w.]+)\s+\((?<{nameof(EndpointInfo.Library)}>\S+)\)";
        return Regex.Match(displayName, pattern);
    }

}