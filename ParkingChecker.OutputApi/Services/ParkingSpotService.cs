using ParkingChecker.OutputApi.Base.Commands;
using ParkingChecker.OutputApi.Entities;
using ParkingChecker.OutputApi.Models.ParkingSpotModels;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingChecker.OutputApi.Services
{
    public interface IParkingSpotService
    {
        Task<FilterParkingSpotsResponse> Filter(string name, int skip, int top);
        Task<GetAllParkingSpotsResponse> GetAll();
        Task<GetParkingSpotByIdResponse> GetById(string id);
    }
    public class ParkingSpotService : IParkingSpotService
    {
        private readonly IBaseGetByIdCommand<ParkingSpot> _getParkingSpotByIdCommand;
        private readonly IBaseGetAllCommand<ParkingSpot> _getAllParkingSpotsCommand;
        private readonly IBaseFilterWithDefinitionsAndCountCommand<ParkingSpot> _filterParkingSpotsCommand;

        public ParkingSpotService(
            IBaseGetByIdCommand<ParkingSpot> getParkingSpotByIdCommand,
            IBaseGetAllCommand<ParkingSpot> getAllParkingSpotsCommand,
            IBaseFilterWithDefinitionsAndCountCommand<ParkingSpot> filterParkingSpotsCommand)

        {
            _getParkingSpotByIdCommand = getParkingSpotByIdCommand;
            _getAllParkingSpotsCommand = getAllParkingSpotsCommand;
            _filterParkingSpotsCommand = filterParkingSpotsCommand;
        }

        public async Task<FilterParkingSpotsResponse> Filter(string name, int skip, int top)
        {
            var res = await _filterParkingSpotsCommand.ExecuteAsync(name, skip, top, x => x.CreationDate, false);
            var response = new FilterParkingSpotsResponse();
            response.Items = res.Items;
            response.Count = res.Count;
            return response;
        }

        public async Task<GetAllParkingSpotsResponse> GetAll()
        {
            var response = new GetAllParkingSpotsResponse();
            response.Items = await _getAllParkingSpotsCommand.ExecuteAsync();
            response.Count = response.Items.Count();
            return response;
        }

        public async Task<GetParkingSpotByIdResponse> GetById(string id)
        {
            var res = await _getParkingSpotByIdCommand.ExecuteAsync(id);
            var item = new GetParkingSpotByIdResponse();
            item.ParkingSpot = res;
            return item;
        }
    }
}
