namespace SparkPlug.Persistence.MongoDb.Models;

[BsonIgnoreExtraElements]
[BsonDiscriminator(RootClass = true)]
public abstract class BaseEntity<TId> : IBaseEntity<TId>
{
#pragma warning disable CS8618
    [BsonElement("_id")]
    [BsonId]
    [BsonIgnoreIfDefault]
    [BsonRepresentation(BsonType.ObjectId)]
    public virtual TId Id { get; set; }
#pragma warning disable CS8618
    public virtual TId GetId()
    {
        return Id;
    }
}
