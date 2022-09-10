using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingChecker.OutputApi.Base.DataAccess;


namespace ParkingChecker.OutputApi.Base.Commands
{
    public interface IBaseGetAllCommand<TDocument> where TDocument : IDocument
    {
        Task<IEnumerable<TDocument>> ExecuteAsync();
    }

    public class BaseGetAllCommand<TDocument> : IBaseGetAllCommand<TDocument> where TDocument : IDocument
    {
        private readonly IBaseRepository<TDocument> _repository;

        public BaseGetAllCommand(IBaseRepository<TDocument> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TDocument>> ExecuteAsync()
        {
            return await _repository.FilterByAsync(x => !x.Deleted);
        }
    }
}