all: clean format build tests
	make packages -B
	make docfx -B

format:
	- dotnet format

clean:
	-dotnet clean
	-rimraf docs

build: build-src build-modules build-samples

build-src:
	-dotnet build ./src/SparkPlug.Source.sln

build-modules:
	-dotnet build ./samples/samples.sln

build-samples:
	-dotnet build ./samples/samples.sln

tests:
	-dotnet test ./tests/SparkPlug.Test.sln

packages:
	-dotnet pack -c release  -o ./packages

docfx:
	-docfx docfx/docfx.json

docs:
	-docfx docfx/docfx.json --serve

outdated:
	-dotnet outdated

deprecated:
	-dotnet list package --deprecated

nuget-clear:
	-dotnet nuget locals all --clear

swagger:
	-swagger tofile --output open-api.json ./samples/Api.Module.Sample/bin/Debug/net8.0/Api.Module.Sample.dll v1

angular:
	-npx @openapitools/openapi-generator-cli generate -i open-api.json -g typescript-angular -o client/angular
	-npx @openapitools/openapi-generator-cli generate -i open-api.json -g dotnet -o client/dotnet

run-sample:
	-setx ASPNETCORE_ENVIRONMENT "Development"
	-dotnet run --project ./samples/Api.Module.Sample/Api.Module.Sample.csproj

run:
	-setx ASPNETCORE_ENVIRONMENT "Development"
	-dotnet run --project ./src/SparkPlug.Hosts/SparkPlug.Hosts.csproj

sdkk:
	setx ASPNETCORE_ENVIRONMENT "Development"
	dotnet run --project ./sdk/ClientSdkGenerator/ClientSdkGenerator.csproj	

migration:
	dotnet ef migrations add InitialCreate --startup-project ./sdk/ClientSdkGenerator --project ./src/SparkPlug.DesignTimeMigration --context HomeDbMigrationContext

update:
	dotnet tool list -g
	dotnet tool update -g coverlet.console                
	dotnet tool update -g docfx                           
	dotnet tool update -g dotnet-counters                 
	dotnet tool update -g dotnet-coverage                 
	dotnet tool update -g dotnet-doc                      
	dotnet tool update -g dotnet-ef                       
	dotnet tool update -g dotnet-format                   
	dotnet tool update -g dotnet-monitor                  
	dotnet tool update -g dotnet-sonarscanner             
	dotnet tool update -g dotnet-version-cli
	dotnet tool update -g swashbuckle.aspnetcore.cli
	dotnet tool update -g versionize
	dotnet tool update -g dotnet-outdated-tool
	dotnet tool update -g nbgv
	dotnet tool list -g

install:
	dotnet tool list -g
	-dotnet tool install -g	coverlet.console                
	-dotnet tool install -g	docfx                           
	-dotnet tool install -g	dotnet-counters                 
	-dotnet tool install -g	dotnet-coverage                 
	-dotnet tool install -g	dotnet-doc                      
	-dotnet tool install -g	dotnet-ef                       
	-dotnet tool install -g	dotnet-format                   
	-dotnet tool install -g	dotnet-monitor                  
	-dotnet tool install -g	dotnet-sonarscanner             
	-dotnet tool install -g	dotnet-version-cli
	-dotnet tool install -g	swashbuckle.aspnetcore.cli
	-dotnet tool install -g	versionize
	-dotnet tool install -g	dotnet-outdated-tool
	-dotnet tool install -g	nbgv
	dotnet tool list -g

setup: install
	-npm install -g rimraf

host:
	dotnet build ./src/SparkPlug.Hosts/SparkPlug.Hosts.csproj
