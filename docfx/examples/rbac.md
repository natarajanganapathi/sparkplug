# RBAC Spec

# Models

Resources
1. Id
2. Resource
// 3. Verb
4. Scops

## Ex:
Id  scope   scope-group     resource
----------------------------------------------------------
1   read     user-mgt       get /user/{id}, 
2   write    user-mgt       post /user/{id}
3   read     user-mgt       get /user/{id}/profile
3   manage   user-mgt       get /user/{id}/profile

Id  scopes                  scope-group    resource
----------------------------------------------------------
1   read, write, manage     user-mgt       /user/{id} 





Scope
Read,
Query,
Write,
Message,



Users
1. Id
2. User

Groups
1. Id
2. Group

UserGroups
1. UserId
2. GroupId

Roles
1. Id
2. Name


RoleResourcePermissionMap


Permissions
1. Id




# Tools:

## Platform tools

1. Kubernetes
2. Helm
3. OPA - Open Policy Agent
4. Redis / TIKV / Inmemroy Cache
5. Postgre SQL Database

## observability and monitoring

1. OpenTelemetry Instrumentation Libraries: These libraries are integrated into your application code to automatically generate telemetry data, including traces, metrics, and contextual information. They provide the foundation for collecting observability data from your services.

2. OpenTelemetry Collector: The collector is a central component that receives, processes, and exports telemetry data. It can be configured to gather data from OpenTelemetry-instrumented applications and then route that data to different backends for further analysis and visualization.

3. Jaeger or Zipkin: These distributed tracing systems can receive trace data from the OpenTelemetry Collector. They allow you to visualize the flow of requests across your microservices, identify bottlenecks, and analyze the latency of individual service calls.

4. Prometheus: Prometheus can be used alongside OpenTelemetry for metrics collection and monitoring. The OpenTelemetry Collector can export metrics data to Prometheus, which can then be used to create dashboards, set up alerts, and perform long-term metrics analysis.

5. Grafana: Grafana can be used to visualize telemetry data collected by various sources, including Jaeger, Prometheus, and other OpenTelemetry exporters. You can create custom dashboards to display traces, metrics, and logs, providing a unified view of your system's observability data.

6. Loki / Elasticsearch : While not directly part of OpenTelemetry, Loki can complement your observability stack by collecting logs and integrating with Grafana for log visualization and analysis.

When deploying these tools together, consider the following:

1. Integration and Compatibility: Ensure that the versions of the tools you choose are compatible with each other. Check the documentation and recommended setups for any integration considerations.

2. Resource Utilization: Keep in mind the resource requirements of each tool when deploying them. Distributed tracing systems like Jaeger and metrics databases like Prometheus can consume resources, so proper resource planning is important.

3. Configuration and Setup: Each tool may require configuration and setup. Plan how data flows between the tools, configure exporters, and create appropriate dashboards.

4. Monitoring and Maintenance: Regularly monitor the health and performance of your observability stack. Update the tools and configurations as needed to adapt to changes in your application and infrastructure.



