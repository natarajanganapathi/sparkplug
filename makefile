build: 
	dotnet build SparkPlug.sln
	dotnet build ./samples/samples.sln
	dotnet build ./tests/SparkPlug.Test.sln

tests:
	dotnet test SparkPlug.sln

pack:
	dotnet pack -c release  -o ./packages

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

sample:
	dotnet build ./samples/samples.sln

docs:
	docfx docfx/docfx.json

serve:
	docfx docfx/docfx.json --serve

swagger:
	swagger tofile --output open-api.json ./samples/Api.Module.Sample/bin/Debug/net7.0/Api.Module.Sample.dll v1

angular:
	npx @openapitools/openapi-generator-cli generate -i open-api.json -g typescript-angular -o client/angular
	npx @openapitools/openapi-generator-cli generate -i open-api.json -g dotnet -o client/dotnet

run-sample:
	setx ASPNETCORE_ENVIRONMENT "Development"
	dotnet run --project ./samples/Api.Module.Sample/Api.Module.Sample.csproj

run:
	setx ASPNETCORE_ENVIRONMENT "Development"
	dotnet run --project ./src/SparkPlug.Hosts/SparkPlug.Hosts.csproj

sdkk:
	setx ASPNETCORE_ENVIRONMENT "Development"
	dotnet run --project ./sdk/ClientSdkGenerator/ClientSdkGenerator.csproj	

mig:
	dotnet ef migrations add InitialCreate --startup-project ./sdk/ClientSdkGenerator --project ./src/SparkPlug.DesignTimeMigration --context HomeDbMigrationContext

tools:
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

setup:
	dotnet tool install --global dotnet-outdated-tool	
	dotnet tool install -g nbgv
