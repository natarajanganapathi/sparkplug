namespace SparkPlug.Persistence.Abstractions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class IgnoreMultiTenantAttribute : Attribute { }