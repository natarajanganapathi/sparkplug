
## HTTPS Certificate for Development
```sh 
dotnet dev-certs https --trust

```

## Optimized build

`<PublishTrimmed>true</PublishTrimmed>` will gives more compiletime waring. more works required.

```xml
<PropertyGroup>
    <PublishTrimmed>true</PublishTrimmed>
    <PublishSingleFile>true</PublishSingleFile>
    <SelfContained>true</SelfContained>
</PropertyGroup>
```


# Nuget Package publish 

1. [Reference](https://learn.microsoft.com/en-us/nuget/nuget-org/publish-a-package)

The project file should be like below. Readme and license file should be included in the project file and the path should have these files. 

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <AssemblyVersion>0.0.0.1</AssemblyVersion>
    <FileVersion>0.0.0.1</FileVersion>
    <Version>0.0.1-alpha1</Version>
    <PackageReadmeFile>README.md</PackageReadmeFile>
    <PackageLicenseFile>LICENSE.txt</PackageLicenseFile>
  </PropertyGroup>
  <ItemGroup>
  </ItemGroup>
  <ItemGroup>
      <None Include="README.md" Pack="true" PackagePath="README.md"/>
      <None Include="LICENSE.txt" Pack="true" PackagePath="LICENSE.txt"/>
  </ItemGroup>
</Project>

```