using MongoDbGenericRepository.Attributes;
using ParkingChecker.OutputApi.Base.DataAccess;

namespace ParkingChecker.OutputApi.Entities
{
    [CollectionName("parking_spot")]
    public class ParkingSpot : Document
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public string ParkingId { get; set; }
        public int VerificationCount { get; set; }
        public bool Available { get; set; }
    }
}
