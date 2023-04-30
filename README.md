# sparkplug

## Dotnet Tools

The following global tools are required

```sh
sparkplug> dotnet tool list -g

Package Id                      Version         Commands
-------------------------------------------------------------------
coverlet.console                3.2.0           coverlet
docfx                           2.63.1          docfx
dotnet-counters                 6.0.351802      dotnet-counters
dotnet-coverage                 17.6.11         dotnet-coverage
dotnet-doc                      1.0.4           docs
dotnet-ef                       7.0.4           dotnet-ef
dotnet-format                   5.1.250801      dotnet-format
dotnet-monitor                  6.3.0           dotnet-monitor
dotnet-sonarscanner             5.12.0          dotnet-sonarscanner
dotnet-version-cli              2.3.1           dotnet-version
swashbuckle.aspnetcore.cli      6.5.0           swagger
versionize                      1.18.0          versionize
```


Module Dependency
=============================================
SparkPlug.<Module>.Domain
    SparkPlug.Persistence.Abstractions


SparkPlug.<Module>.Repository.Sql
    SparkPlug.Persistence.EntityFramework
    SparkPlug.<Module>.Domain


SparkPlug.<Module>.Repository.Mongo
    SparkPlug.Persistence.Mongo
    SparkPlug.<Module>.Domain


SparkPlug.<Module>.Service
    SparkPlug.Contracts
    SparkPlug.Persistence.Abstractions
    SparkPlug.<Module>.Domain


SparkPlug.<Module>.Api
    SparkPlug.Contracts
    SparkPlug.Api

Application
===============================================
SparkPlug.<ApplicationName>.Host
    SparkPlug.<Module-1>.Api
    SparkPlug.<Module-2>.Api
    SparkPlug.<Module-2>.Api