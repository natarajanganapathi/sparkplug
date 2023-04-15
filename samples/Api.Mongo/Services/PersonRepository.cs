namespace SparkPlug.Sample.WebApi.Repositories;

public class PersonService : BaseService<string, Person>
{
    public PersonService(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
