build: 
	dotnet build SparkPlug.sln
	dotnet build ./samples/samples.sln
	dotnet build ./tests/SparkPlug.Test.sln

tests:
	dotnet test SparkPlug.sln

pack:
	dotnet pack -c release  -o .

clean:
	dotnet clean

find-deprecated:
	dotnet list package --deprecated

nuget-clear:
	dotnet nuget locals all --clear

full:
	make clean
	make build
	make tests
	make pack

samples:
	dotnet build ./sample/WebApi/WebApi.csproj

docs:
	docfx docfx/docfx.json

serve:
	docfx docfx/docfx.json --serve

swagger:
	swagger tofile --output open-api.json ./samples/Api.Module.Sample/bin/Debug/net7.0/Api.Module.Sample.dll v1

angular:
	npx @openapitools/openapi-generator-cli generate -i open-api.json -g typescript-angular -o client/angular
	npx @openapitools/openapi-generator-cli generate -i open-api.json -g dotnet -o client/dotnet
