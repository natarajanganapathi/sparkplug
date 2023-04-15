namespace SparkPlug.Sample.WebApi.Repositories;

public class PersonService : BaseService<long, Person>
{
    public PersonService(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
