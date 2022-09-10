using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ParkingChecker.OutputApi.Base.DataAccess
{
    public abstract class Document : IDocument
    {
        [BsonId]  
        [BsonRepresentation(BsonType.ObjectId)]  
        [JsonIgnore]
        public string Id { get; set; }

        [JsonIgnore]
        public DateTime CreationDate { get; set; }
        
        [JsonIgnore]
        public bool Deleted { get; set; }
        
        [JsonIgnore]
        public DateTime DeletionDate { get; set; }

        protected Document()
        {
            Deleted = false;
            DateTime now = DateTime.UtcNow;
            CreationDate = new DateTime(now.Ticks / 100000 * 100000, now.Kind);
        }
    }
}