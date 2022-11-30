using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SampleMongoApp.Entities
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity : IEntity
    {
       
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public virtual string Id { get; set; }
    }

    public interface IEntity
    {
        [BsonId]
        string Id { get; set; }
    }
}
