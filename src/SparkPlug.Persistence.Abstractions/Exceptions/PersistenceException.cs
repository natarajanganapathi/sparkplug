namespace SparkPlug.Persistence.Abstractions;

[Serializable]
public class PersistenceException : Exception
{
    public PersistenceException() { }
    public PersistenceException(string? message) : base(message) { }
    public PersistenceException(string? message, Exception? innerException) : base(message, innerException) { }
    protected PersistenceException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

[Serializable]
public class CreateEntityException : PersistenceException
{
    public CreateEntityException() { }
    public CreateEntityException(string? message) : base(message) { }
    public CreateEntityException(string? message, Exception? innerException) : base(message, innerException) { }
    protected CreateEntityException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

[Serializable]
public class DeleteEntityException : PersistenceException
{
    public DeleteEntityException() { }
    public DeleteEntityException(string? message) : base(message) { }
    public DeleteEntityException(string? message, Exception? innerException) : base(message, innerException) { }
    protected DeleteEntityException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

[Serializable]
public class QueryEntityException : PersistenceException
{
    public QueryEntityException() { }
    public QueryEntityException(string? message) : base(message) { }
    public QueryEntityException(string? message, Exception? innerException) : base(message, innerException) { }
    protected QueryEntityException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

[Serializable]
public class UpdateEntityException : PersistenceException
{
    public UpdateEntityException() { }
    public UpdateEntityException(string? message) : base(message) { }
    public UpdateEntityException(string? message, Exception? innerException) : base(message, innerException) { }
    protected UpdateEntityException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
