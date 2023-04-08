namespace SparkPlug.Api.Controllers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class IgnoreMultiTenantAttribute : Attribute { }