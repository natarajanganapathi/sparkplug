namespace SparkPlug.Persistence.Abstractions;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class KeyAttribute : Attribute
{
    public KeyAttribute(int order)
    {
        Order = order;
    }

    public int Order { get; set; }
}
