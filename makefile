build: 
	dotnet build SparkPlug.sln
	dotnet build ./samples/
	dotnet build ./tests/

tests:
	dotnet test SparkPlug.sln

pack:
	dotnet pack -c release  -o .

clean:
	dotnet clean

full: 
	make clean
	make build
	make tests
	make pack

samples:
	dotnet build ./sample/WebApi/WebApi.csproj

docs:
	docfx --header-file yamlheader.yml docfx/docfx.json

serve:
	docfx --header-file yamlheader.yml docfx/docfx.json --serve

swagger:
	swagger tofile --output open-api.json ./samples/Api.Module.Sample/bin/Debug/net7.0/Api.Module.Sample.dll v1

angular:
	npx @openapitools/openapi-generator-cli generate -i openapi.json -g typescript-angular -o client
