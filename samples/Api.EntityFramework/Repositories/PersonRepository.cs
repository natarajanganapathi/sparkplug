namespace SparkPlug.Sample.WebApi.Repositories;

public class PersonRepository : Repository<long, Person>
{
    public PersonRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
