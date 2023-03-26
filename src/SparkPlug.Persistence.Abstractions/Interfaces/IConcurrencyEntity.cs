namespace SparkPlug.Persistence.Abstractions;

public interface IConcurrencyEntity
{
    int Revision { get; set; }
}
