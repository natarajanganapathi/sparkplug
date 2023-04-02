namespace SparkPlug.Sample.WebApi.Models;

[Api("tenants", typeof(ApiController<,>))]
public class Tenants<Guid> : TenantDetails { }