using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ParkingChecker.OutputApi.Base.DataAccess;

namespace ParkingChecker.OutputApi.Base.Commands
{
    public interface IBaseFindOneCommand<TDocument> where TDocument : IDocument
    {
        Task<TDocument> ExecuteAsync(Expression<Func<TDocument, bool>> expression);
    }

    public class BaseFindOneCommand<TDocument> : IBaseFindOneCommand<TDocument> where TDocument : IDocument
    {
        private readonly IBaseRepository<TDocument> _repository;

        public BaseFindOneCommand(IBaseRepository<TDocument> repository)
        {
            _repository = repository;
        }

        public async Task<TDocument> ExecuteAsync(Expression<Func<TDocument, bool>> expression)
        {
            var document = (await _repository.FilterByAsync(expression)).FirstOrDefault(x => !x.Deleted);
            return document;
        }
    }
}