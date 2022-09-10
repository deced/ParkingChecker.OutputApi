using Microsoft.AspNetCore.Mvc;
using ParkingChecker.OutputApi.Models.ParkingSpotModels;
using ParkingChecker.OutputApi.Services;
using System.Threading.Tasks;

namespace ParkingChecker.OutputApi.Controllers
{
    [Route("[controller]/[action]")]
    public class ParkingSpotController : Controller
    {
        private readonly IParkingSpotService _parkingSpotService;
        public ParkingSpotController(IParkingSpotService parkingSpotService)
        {
            _parkingSpotService = parkingSpotService;
        }

        [HttpGet]
        public async Task<FilterParkingSpotsResponse> Filter([FromQuery] string filter, [FromQuery] int skip, [FromQuery] int top)
        {
            return await _parkingSpotService.Filter(filter, skip, top);
        }

        [HttpGet]
        public async Task<GetAllParkingSpotsResponse> GetAll()
        {
            return await _parkingSpotService.GetAll();
        }

        [HttpGet]
        public async Task<GetParkingSpotByIdResponse> GetById(string id)
        {
            return await _parkingSpotService.GetById(id);
        }
    }
}
