using ParkingChecker.OutputApi.Entities;
using System.Collections.Generic;

namespace ParkingChecker.OutputApi.Models.ParkingModels
{
    public class FilterParkingsResponse
    {
        public IEnumerable<Parking> Items { get; set; }
        public long Count { get; set; }
    }
}
