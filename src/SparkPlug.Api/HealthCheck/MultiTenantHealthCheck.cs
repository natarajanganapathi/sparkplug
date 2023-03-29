// namespace SparkPlug.Persistence.Api.HealthCheck;

// public class MultiTenantHealthCheck : IHealthCheck
// {
//     //     private readonly ITenantProvider _tenantProvider;

//     //     public MultiTenantHealthCheck(ITenantProvider tenantProvider)
//     //     {
//     //         _tenantProvider = tenantProvider;
//     //     }

//     public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
//     {
//         //         var results = new Dictionary<string, HealthCheckResult>();
//         //         foreach (var tenant in _tenantProvider.GetAllTenants())
//         //         {
//         //             try
//         //             {
//         //                 // Perform health check for the tenant's database
//         //                 var result = await CheckDatabaseHealthAsync(tenant.DatabaseConnectionString);
//         //                 // Add the result to the dictionary
//         //                 results.Add(tenant.TenantId.ToString(), result);
//         //             }
//         //             catch (Exception ex)
//         //             {
//         //                 // If there is an error, add a failed result to the dictionary
//         //                 results.Add(tenant.TenantId.ToString(), new HealthCheckResult(context.Registration.FailureStatus, exception: ex));
//         //             }
//         //         }
//         //         if (results.Count == 0)
//         //         {
//         //             return new HealthCheckResult(context.Registration.FailureStatus, "No tenants found");
//         //         }
//         //         else if (results.Values.Any(r => r.Status != HealthStatus.Healthy))
//         //         {
//         //             return new HealthCheckResult(context.Registration.FailureStatus, data: results);
//         //         }
//         //         else
//         //         {
//         //             return HealthCheckResult.Healthy("All tenants are healthy", data: results);
//         //         }
//         return await Task.FromResult(HealthCheckResult.Healthy("All tenants are healthy"));
//     }

//     //     private async Task<HealthCheckResult> CheckDatabaseHealthAsync(string connectionString)
//     //     {
//     //         // Perform health check for the database using the connection string
//     //         // ...
//     //         return HealthCheckResult.Healthy("Database is healthy");
//     //     }
// }
