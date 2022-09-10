using MongoDbGenericRepository.Attributes;
using ParkingChecker.OutputApi.Base.DataAccess;

namespace ParkingChecker.OutputApi.Entities
{
    [CollectionName("parking")]
    public class Parking : Document
    {
        public string InputFile { get; set; }
    }
}
