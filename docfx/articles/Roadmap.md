# SparkPlug Roadmap

## Framework Development
1. Storage Abstraction and Azure storage Implementation to access and manage the files.
2. Puppeteer Sharp project for PDF generation
3. Common string extenstion for string Case chagse. ex. camelCase, Snake_Case, PascalCase, etc.

### SparkPlug.Api
1. Load SparkPlug dependency module dynamically
2. Composite Api Controller Template development for Composite Key tables - Workaround is sarogate Id for Composite Key tables.
3. Data Cache implementation                                                                                                        - Done
4. CompositeRequest handling api development                                                                                        - InProgress
5. Enable Http 2.0/3.0 in WebApi
6. Database.EnsureCreated() move to Create Tenant api - When onboard new tenant, this will create the db schema.
7. Database.Migrate() should be called in application Startup.

### SparkPlug.Persistance
1. 

### SparkPlug.Persistance.EntityFramework
1. Repository should support Composite Key tables
2. In `QueryRequeset`, add `Exclude[]` property to exclude properties in response. If we provide `Exclude[]` without select, It returns all the columns except Exclude properties. 
3. Filter criteria need to add in all the `QueryRequest` for each entity to get only `live` data.
4. Filter criteria for included entity add in QueryRequest. - Done for where condition. 
5. Support `Not` operator for all cobinations of `Filter`. Like `NotIn`, `NotEq`, `NotNull`, etc.
6. DB Migrations using ef migrate command.

## Sample Application Development
1. Azure Authentication integration
2. Azure Application Insights integration
3. Sample project for Single Tenant and Multi Tenant

## Commandline Interface (CLI) Development
1. Razor Template Engine Module. Don't use RazorLight dll for template engine.
2. CLI Base Framework with help of `Spectre.Console`
3. CLI for `SparkPlug`

## Infrastructure Development
1. OPA (Open Policy Agent) based authorization module
2. Add local machie as Azure Kubernetes worker node 
3. Istio setup in kubernetes environment

## DevOps Development
1. Github Actions for CICD                                                                      - In Progress
2. Create GitHub Actions pipeline for generating SKD from OpenAPI 3.0 json file
3. Dockerfile for dotnet 8 with AOT Compilation for apis.

## To be validate
1. `Json.NET (Newtonsoft.Json) ` can be replaced by `System.Text.Json`. 
    * Which framework is providing high performance
    * Which framework is supporting Expression based query object construction. JSON.NET supportign and implemented. 
    * Child object property name also need to be in camael case in HTTP Response.

2. 