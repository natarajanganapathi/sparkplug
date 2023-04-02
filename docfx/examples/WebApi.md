# SparkPlug.Api

## Focused Area in this framework

1. Authentication and authorization: Ensure that only authorized users have access to the API.
2. Security: Use secure protocols such as SSL/TLS and implement security measures such as input validation, rate limiting, and firewalls.
3. Scalability: Ensure that the API can handle large amounts of traffic and handle it efficiently.
4. Performance: Optimize the API's performance to reduce response times and increase efficiency.
5. Error handling: Implement robust error handling to ensure that the API can handle errors gracefully and provide meaningful error messages to the clients.
6. Documentation: Provide clear and comprehensive documentation for the API, including API specifications and examples of how to use the API.
7. Versioning: Implement API versioning to ensure that older clients can continue to work with the API even as new features are added.
8. Monitoring and logging: Monitor the API's performance and usage to detect and diagnose issues, and log important events to help diagnose problems and improve the API.


1. Program.cs               - Done
2. Startup.cs               - Done
3. OpenApi Configuration    - Done
4. Dependecny Injection     - Done
    - Dotnet core DI
5. Logging
    - Serilog
    - SimpleConsole
6. Tracing
    - Azure Application Insights
7. Configuration
    - appsettings.json               - Done
    - Azure Vault Configuration      - 
    - Kubernetes Config Map          -
8. Filters
    - Global Exception Filter        - Done
    - Authentication Filter
9. Exception Handling                - Done
10. Health Check 
    - Database Health Check          - Done
    - Local Resource Manual Health check
    - Azure Service's Health Check   
11. Monitoring
    - Azure Monitor
12. Entity Framework
    - Code First
        - Create DB Schema          - Done
        - Create Migration Script
        - Execute Migration Script
    - Db First
    - Configuration
    - Validation 
13. DDD - Domain Driven Design
14. CQRS
15. MediatR
16. Service Bus Integration
    - Azure Service Bus
    - Rabbit MQ
    - Kafka
17. - Multi-Tenancy                 - Done
17. CORS
18. CI/CD pipleline                 - Done
    - Github Actions                - Done
19. Container
    - Docker Build
    - Helm Chart
20. Caching 
    - Distributed Memory Cache
    - Distributed Redis Cache
21. REST Api Maturity Model
