using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ParkingChecker.OutputApi.Base.DataAccess;
using ParkingChecker.OutputApi.Helpers;

namespace ParkingChecker.OutputApi.Base.Commands
{
    public interface IBaseCountCommand<TDocument> where TDocument : IDocument
    {
        Task<long> ExecuteAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
    public class BaseCountCommand<TDocument> :IBaseCountCommand<TDocument> where TDocument : IDocument
    {
        private readonly IBaseRepository<TDocument> _repository;

        public BaseCountCommand(IBaseRepository<TDocument> repository)
        {
            _repository = repository;
        }
        public async Task<long> ExecuteAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            Expression<Func<TDocument, bool>> filterDeletedExpression = x => x.Deleted == false;
            return await _repository.FilterCountByAsync(filterExpression.And(filterDeletedExpression));
        }
    }
}