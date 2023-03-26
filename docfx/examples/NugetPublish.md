# Nuget Package publish 

The project file should be like below. Readme and license file should be included in the project file and the path should have these files. 

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net7.0</TargetFramework>
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