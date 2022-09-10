using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ParkingChecker.OutputApi.Base.DataAccess
{
    public interface IBaseRepository<TDocument> where TDocument : IDocument
    {
        FilterDefinitionBuilder<TDocument> FilterDefinitionBuilder { get; }
        SortDefinitionBuilder<TDocument> SortDefinitionBuilder { get; }
        IQueryable<TDocument> AsQueryable();

        IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression);

        Task<IEnumerable<TDocument>> FilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression);
        Task<IEnumerable<TDocument>> SortFilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, object>> sortObject);
        Task<IEnumerable<TDocument>> LimitFilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression, int take);
        Task<IEnumerable<TDocument>> SortLimitFilterByAsync(
           Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, object>> sortObject, int skip, int top);
        Task<IEnumerable<TDocument>> SortByDescendingLimitFilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, object>> sortObject, int skip, int top);
        Task<IEnumerable<TDocument>> FindManySortAndUpdateAsync(
            Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, object>> sortObject, UpdateDefinition<TDocument> updateDefinition, int count);
        Task<long> FilterCountByAsync(
            Expression<Func<TDocument, bool>> filterExpression);
        Task<IEnumerable<TDocument>> GetAllAsync();
        List<FilterDefinition<TDocument>> GetFilterDefinitionsFromString(string filterString);

        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> FindByIdAsync(string id);
        Task<IEnumerable<TDocument>> FilterWithSkipAsync(IEnumerable<FilterDefinition<TDocument>> filterDefinitions, int skip, int count, SortDefinition<TDocument> sortDefinition = null);

        Task<long> GetCountAsync(IEnumerable<FilterDefinition<TDocument>> filterDefinitions);
    }
}