using System;
using MongoDbGenericRepository.Attributes;
using ParkingChecker.OutputApi.Base.DataAccess;

namespace ParkingChecker.OutputApi.Entities
{
    [CollectionName("parking_spot")]
    public class ParkingSpot : Document
    {
        public int x1 { get; set; }
        public int y1 { get; set; }
        public int x2 { get; set; }
        public int y2 { get; set; }
        public int verificationCount { get; set; }
        public bool available { get; set; }
        public bool approved { get; set; }
        public DateTime lastUpdate { get; set; }
        public string parkingId { get; set; }
    }
}