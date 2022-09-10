using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ParkingChecker.OutputApi.Base.DataAccess
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }
        DateTime CreationDate { get; set; }
        bool Deleted { get; set; }
        DateTime DeletionDate { get; set; }
    }
}