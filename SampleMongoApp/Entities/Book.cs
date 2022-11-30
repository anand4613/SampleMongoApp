using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SampleMongoApp.Entities
{

    [BsonIgnoreExtraElements]
    public class Book:Entity
    {
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [BsonElement("category")]
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [BsonElement("author")]
        [JsonPropertyName("author")]
        public string Author { get; set; }
    }
}
