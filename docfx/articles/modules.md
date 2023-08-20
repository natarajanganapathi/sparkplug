# SparkPlug Module

```C#
namespace SparkPlug.Persistence.Abstractions;

public interface IModule
{
    void AddModule(IServiceCollection services);
    void UseModule(IApplicationBuilder app, IServiceProvider serviceProvider);
    void UseMiddelwares(IApplicationBuilder app);
}
```

1. `AddModule(IServiceCollection services)`: This method is responsible for configuring services that the module requires. You can use this method to register services, repositories, and other dependencies that your module needs to function properly. For example, if your module requires database access, you could register database-related services here.

2. `UseModule(IApplicationBuilder app, IServiceProvider serviceProvider)`: This method is intended to be used during application startup to configure the module's behavior. You can use this method to set up routes, controllers, and other components that your module provides. It's important to note that you should be cautious about modifying the app parameter, as it affects the global application configuration.

3. `UseMiddlewares(IApplicationBuilder app)`: This method seems to be used for setting up any middlewares specific to your module. Middlewares are components that can handle requests and responses, and they are typically placed in the request/response pipeline. This allows you to perform various tasks, such as authentication, logging, and exception handling. This method would provide a way to add middleware components specific to your module.
