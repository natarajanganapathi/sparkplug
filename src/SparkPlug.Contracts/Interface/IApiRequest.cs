namespace SparkPlug.Contracts;

public interface IApiRequest
{
    public string[]? Deps { get; set; }
}

public interface IInclude
{
    public string Name { get; set; }
    public string[] Select { get; set; }
    public Include[] Includes {get;set;}
}

public interface IQueryRequest : IApiRequest
{
    string[]? Select { get; set; }
    Include[]? Includes { get; set; }
    Filter? Where { get; set; }
    Order[]? Sort { get; set; }
    PageContext? Page { get; set; }
}

public interface ICommandRequest<TEntity> : IApiRequest
{
    TEntity? Data { get; set; }
}

public interface ICompositeRequest : IApiRequest
{
    Dictionary<string, IApiRequest>? Requests { get; set; }
}