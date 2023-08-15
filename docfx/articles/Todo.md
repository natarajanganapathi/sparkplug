# ToDo List 

## SparkPlug.Api
1. Load SparkPlug dependency module dynamically
2. Composite Api Controller Template development for Composite Key tables - Workaround is sarogate Id for Composite Key tables.
3. Data Cache implementation - Done
4. CompositeRequest handling api development - InProgress
5. Enable Http 2.0/3.0 in WebApi
6. Database.EnsureCreated() move to Create Tenant api - When onboard new tenant, this will create the db schema.
7. Database.Migrate() should be called in application Startup.

## SparkPlug.Persistance
1. 

## SparkPlug.Persistance.EntityFramework
1. Repository should support Composite Key tables
2. In `QueryRequeset`, add `Exclude[]` property to exclude properties in response. If we provide `Exclude[]` without select, It returns all the columns except Exclude properties. 
3. Filter criteria need to add in all the `QueryRequest` for each entity to get only `live` data.
4. Filter criteria for included entity add in QueryRequest. - Done for where condition. 
5. Support `Not` operator for all cobinations of `Filter`. Like `NotIn`, `NotEq`, `NotNull`, etc.
6. DB Migrations using ef migrate command.

## Sample Projects 
1. Azure Authentication integration
2. Azure Application Insights integration

## Common
1. Common string extenstion for string Case chagse. ex. camelCase, Snake_Case, PascalCase, etc.
2. Github Actions for CICD - In Progress
3. Razor Template Engine Module 
4. Puppeteer Sharp project for PDF generation
5. Dockerfile for dotnet 8 with AOT Compilation for apis.
6. OPA (Open Policy Agent) based authorization module