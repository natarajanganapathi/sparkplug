build: 
	dotnet build SparkPlug.sln

tests:
	dotnet test SparkPlug.sln

samples:
	dotnet build ./sample/WebApi/WebApi.csproj

docs:
	docfx --header-file yamlheader.yml docfx/docfx.json

serve:
	docfx --header-file yamlheader.yml docfx/docfx.json --serve
