using ParkingChecker.OutputApi.Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ParkingChecker.OutputApi.Base.Commands
{

    public interface IBaseFilterCommand<TDocument> where TDocument : IDocument
    {
        Task<IEnumerable<TDocument>> ExecuteAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
    public class BaseFilterCommand<TDocument> : IBaseFilterCommand<TDocument> where TDocument : IDocument
    {
        private readonly IBaseRepository<TDocument> _repository;

        public BaseFilterCommand(IBaseRepository<TDocument> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TDocument>> ExecuteAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return (await _repository.FilterByAsync(filterExpression)).Where(x => !x.Deleted);
        }
    }
}
