using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ParkingChecker.OutputApi.Models.ParkingModels;
using System.Threading.Tasks;
using ParkingChecker.OutputApi.Base.DataAccess;
using ParkingChecker.OutputApi.Entities;

namespace ParkingChecker.OutputApi.Controllers
{
    [Route("[controller]/[action]")]
    public class ParkingController : Controller
    {
        // private readonly IParkingService _parkingService;
        //
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
        private readonly IBaseRepository<Parking> _parkingRepository;
        private readonly IBaseRepository<ParkingSpot> _parkingSpotRepository;
        private readonly IBaseRepository<OutputImage> _outputImageRepository;

        public ParkingController(IBaseRepository<Parking> parkingRepository,
            IBaseRepository<ParkingSpot> parkingSpotRepository, IBaseRepository<OutputImage> outputImageRepository)
        {
            _parkingRepository = parkingRepository;
            _parkingSpotRepository = parkingSpotRepository;
            _outputImageRepository = outputImageRepository;
        }

        [HttpGet]
        public async Task<List<ParkingModel>> GetListOfParking()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            var parkings = (await _parkingRepository.GetAllAsync()).ToList();

            List<ParkingModel> listOfParkingModel = new List<ParkingModel>();
            for (int i = 0; i < parkings.Count; i++)
            {
                ParkingModel parkingModel = new ParkingModel();
                parkingModel.Id = parkings[i].parkingId;
                parkingModel.Name = parkings[i].name;

                parkingModel.AvailableSpotsCount =
                    (await _parkingSpotRepository.FilterByAsync(spot =>
                        spot.parkingId == parkingModel.Id && spot.available == true)).Count();
                listOfParkingModel.Add(parkingModel);
            }

            return listOfParkingModel;
        }

        [HttpGet("{parkingId}")]
        public async Task<ParkingInfoModel> GetDataAboutAvailableSpots(string parkingId)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            var parking = (await _parkingRepository.FindOneAsync(parking => parking.parkingId == parkingId));
            if (parking == null)
                return null;
            ParkingInfoModel parkingInfoModel = new ParkingInfoModel();
            parkingInfoModel.ParkingName = parking.name;
            parkingInfoModel.AvailableSpotsCount =
                (await _parkingSpotRepository.FilterByAsync(spot =>
                    spot.parkingId == parkingId && spot.available == true)).Count();

            var imagePath = (await _outputImageRepository.FindOneAsync(image => image.parkingId == parkingId))
                ?.fullPath;
            if (imagePath != null)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(imagePath);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                parkingInfoModel.Image = base64ImageRepresentation;
            }
            return parkingInfoModel;
            
        }
    }
}