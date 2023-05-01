# Todo

1. Serializer Implmentation for ApiRequests to send as payload or querystring in SparkPlug.Contract library. 
    a. .ToJson()

2. RESTSharp integration to get or post data to REST API Url in SparkPlug.Contract library. 
    a. .Get("/user/10")
    b. .Post("/user")
    c. .Put("/user")
    d. .Delete("/user/10")
    e. .Patch("/user/10")

3. SparkPlug.Api Unit Testing with MongoDb
5. GenericTypeControllerFeatureProvider - Add controller to feature.Controllers with sapcific api verbs (only GET and POST like). - Feature rejected due to introduce service layer.
8. Generic Cache interface need to be implement for SparkPlug.Api
9. Distributed Cache interface implementation to cache data including Tenant information.
10. Update or reload config if the configuration is modifyed in the cloud.
11. Functional Module list
    a. RBAC based Authorization, UI Screens mappings for role
    b. Multi-Tenancy Management
    c. Workflow engine (Camunda)
12. OpenApi with API Scope declaration and C# attribute to define scope of the API.
13. If table contains Composite Key, Need to handle it. (CompositeApi attribute and [Key(1)], [Key(2)] attribute implement for composite key table.)
17. Database Migration using EF migration support
19. Include Status column in all the select, update, delete queries. 
 

# Done

4. All Rest API make it workable with MongoDb sample Application. - Done
6. Dynamically add api version in Swagger docs. - Done
7. Transactions handling - Done
14. Enable Table Concurrency in API (Rev column need to add) - Done
15. Enable Architecture / Audit columns in APIs.  - Done
16. Soft Delete (status Column) added by IDeletableEntity interface. - Done
18. INCLUDE Foreign table columns in response for EntityFramework. - Done
20. FilterType renamed to Kind  - Done
21. Between field operator need to implement - Done

# Ask 

1. Inside AddService methods, how to access IOptions<SqlDbOptions> instance?
2. Geting error if we call 2 times an api (same context object used.)
3. 