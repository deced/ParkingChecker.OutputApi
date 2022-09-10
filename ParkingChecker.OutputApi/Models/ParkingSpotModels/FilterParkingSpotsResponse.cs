using ParkingChecker.OutputApi.Entities;
using System.Collections.Generic;

namespace ParkingChecker.OutputApi.Models.ParkingSpotModels
{
    public class FilterParkingSpotsResponse
    {
        public IEnumerable<ParkingSpot> Items { get; set; }
        public long Count { get; set; }
    }
}
