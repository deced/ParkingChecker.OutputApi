using ParkingChecker.OutputApi.Base.Commands;
using ParkingChecker.OutputApi.Entities;
using ParkingChecker.OutputApi.Models.ParkingModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingChecker.OutputApi.Services
{
    public interface IParkingService
    {
        Task<FilterParkingsResponse> Filter(string name, int skip, int top);
        Task<GetAllParkingsResponse> GetAll();
        Task<GetParkingByIdResponse> GetById(string id);
    }
    public class ParkingService : IParkingService
    {
        private readonly IBaseGetByIdCommand<Parking> _getParkingByIdCommand;
        private readonly IBaseGetAllCommand<Parking> _getAllParkingsCommand;
        private readonly IBaseFilterWithDefinitionsAndCountCommand<Parking> _filterParkingsCommand;

        public ParkingService(
            IBaseGetByIdCommand<Parking> getParkingByIdCommand,
            IBaseGetAllCommand<Parking> getAllParkingsCommand,
            IBaseFilterWithDefinitionsAndCountCommand<Parking> filterParkingsCommand)

        {
            _getParkingByIdCommand = getParkingByIdCommand;
            _getAllParkingsCommand = getAllParkingsCommand;
            _filterParkingsCommand = filterParkingsCommand;
        }

        public async Task<FilterParkingsResponse> Filter(string name, int skip, int top)
        {
            var res = await _filterParkingsCommand.ExecuteAsync(name, skip, top, x => x.CreationDate, false);
            var response = new FilterParkingsResponse();
            response.Items = res.Items;
            response.Count = res.Count;
            return response;
        }

        public async Task<GetAllParkingsResponse> GetAll()
        {
            var response = new GetAllParkingsResponse();
            response.Items = await _getAllParkingsCommand.ExecuteAsync();
            response.Count = response.Items.Count();
            return response;
        }

        public async Task<GetParkingByIdResponse> GetById(string id)
        {
            var res =  await _getParkingByIdCommand.ExecuteAsync(id);
            var item = new GetParkingByIdResponse();
            item.Parking = res;
            return item;
        }
    }
}
