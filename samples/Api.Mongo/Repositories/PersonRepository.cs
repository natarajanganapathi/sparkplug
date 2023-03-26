namespace SparkPlug.Sample.WebApi.Repositories;

public class PersonRepository : Repository<string, Person>
{
    public PersonRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
