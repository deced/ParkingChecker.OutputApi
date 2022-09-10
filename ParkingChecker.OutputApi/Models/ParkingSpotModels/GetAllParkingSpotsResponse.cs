using ParkingChecker.OutputApi.Entities;
using System.Collections.Generic;

namespace ParkingChecker.OutputApi.Models.ParkingSpotModels
{
    public class GetAllParkingSpotsResponse
    {
        public IEnumerable<ParkingSpot> Items { get; set; }
        public long Count { get; set; }
    }
}
