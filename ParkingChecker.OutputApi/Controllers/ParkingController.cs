using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ParkingChecker.OutputApi.Models.ParkingModels;
using ParkingChecker.OutputApi.Services;
using System.Threading.Tasks;
using ParkingChecker.OutputApi.Entities;

namespace ParkingChecker.OutputApi.Controllers
{
    [Route("[controller]/[action]")]
    public class ParkingController : Controller
    {
     //   private readonly IParkingService _parkingService;

        // public ParkingController(IParkingService parkingService)
        // {
        //     _parkingService = parkingService;
        // }

        [HttpGet]
        public string HealthCheck()
        {
            return "working";
        }

        // [HttpGet]
        // public async Task<FilterParkingsResponse> Filter([FromQuery] string filter, [FromQuery] int skip,
        //     [FromQuery] int top)
        // {
        //     return await _parkingService.Filter(filter, skip, top);
        // }
        //
        // [HttpGet]
        // public async Task<GetAllParkingsResponse> GetAll()
        // {
        //     return await _parkingService.GetAll();
        // }
        //
        // [HttpGet]
        // public async Task<GetParkingByIdResponse> GetById(string id)
        // {
        //     return await _parkingService.GetById(id);
        // }

        [HttpGet]
        public List<ParkingModel> GetListOfParking()
        {
            ParkingModel parkingModel1 = new ParkingModel();
            parkingModel1.Name = "ПАРКОВКА1";
            parkingModel1.Id = "7474774848bdgw938d720d";
            parkingModel1.AvailableSpotsCount = 50;
           

            ParkingModel parkingModel2 = new ParkingModel();
            parkingModel2.Name = "ПАРКОВКА2";
            parkingModel2.Id = "1736g363v6f8f8f8f8f88";
            parkingModel2.AvailableSpotsCount = 20;

            List<ParkingModel> listOfParkingModel = new List<ParkingModel>();
            listOfParkingModel.Add(parkingModel1);
            listOfParkingModel.Add(parkingModel2);
            return listOfParkingModel;
        }

        [HttpGet( "{parkingId}")]
        public ParkingInfoModel GetDataAboutAvailableSpots(string parkingId )
        {
            ParkingInfoModel parkingInfoModel = new ParkingInfoModel();
            parkingInfoModel.ParkingName = "Парковка 1";
            parkingInfoModel.AvailableSpotsCount = 10;
            parkingInfoModel.ImagePath =
                "https://hsto.org/r/w1560/getpro/habr/upload_files/a20/0b2/05b/a200b205b18e4f4cefd3dfb8469c4cea.jpg";
            return parkingInfoModel;
        }
    }
}