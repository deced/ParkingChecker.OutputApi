using ParkingChecker.OutputApi.Base.DataAccess;
using System.Threading.Tasks;

namespace ParkingChecker.OutputApi.Base.Commands
{
    public interface IBaseGetByIdCommand<TDocument> where TDocument : IDocument
    {
        Task<TDocument> ExecuteAsync(string id);
    }

    public class BaseGetByIdCommand<TDocument> : IBaseGetByIdCommand<TDocument> where TDocument : IDocument
    {
        private readonly IBaseRepository<TDocument> _repository;

        public BaseGetByIdCommand(IBaseRepository<TDocument> repository)
        {
            _repository = repository;
        }

        public async Task<TDocument> ExecuteAsync(string id)
        {
            var document = await _repository.FindByIdAsync(id);
            return document.Deleted ? default : document;
        }
    }
}