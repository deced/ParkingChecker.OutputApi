using System;
using MongoDbGenericRepository.Attributes;
using ParkingChecker.OutputApi.Base.DataAccess;

namespace ParkingChecker.OutputApi.Entities
{
    [CollectionName("output_image")]
    public class OutputImage: Document
    {
        public string  fullPath { get; set; }
        public string parkingId { get; set; }
     
    }
}