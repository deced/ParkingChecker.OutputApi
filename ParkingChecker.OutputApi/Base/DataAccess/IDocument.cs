using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ParkingChecker.OutputApi.Base.DataAccess
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string _id { get; set; }
        DateTime creationDate { get; set; }
        
    }
}