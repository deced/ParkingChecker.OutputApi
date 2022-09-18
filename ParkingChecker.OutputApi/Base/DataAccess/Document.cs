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
        public string _id { get; set; }

        [JsonIgnore]
        public DateTime creationDate { get; set; }
        
    }
}