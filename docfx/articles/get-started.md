

## Configuration

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information",
            "SparkPlug.*": "Information"
        }
    },
    "SparkPlug": {
        "Api": {
            "ApplicationName": "Web Api",
            "PathBase": "/api/v1"
        },
        "SqlDb": {
            "ConnectionString": ""
        }
    }
}
```

## Additional connection string attribute required to ignore certificate issue in development
`Trust Server Certificate=true` may required in connection string.

## SparkPlug.Api.PathBase

This property refers to the application base path. If the path contains `{tanant}` then the application can handle multi tenant's request. The Tenat Id will be passed in the URL path. Also `appsettings.json` file connection string consider as tenant managemetn database.

### Single Tenant Application
Ex. "PathBase" : "/api/v1/"

### Multi Tenant Application
Ex. "PathBase" : "/api/v1/{tanant}/"

### TraceIdentifier vs CorrelationId

utilizing `TraceIdentifier` for local request tracking within a service and `CorrelationId` for tracking across services in a distributed environment.