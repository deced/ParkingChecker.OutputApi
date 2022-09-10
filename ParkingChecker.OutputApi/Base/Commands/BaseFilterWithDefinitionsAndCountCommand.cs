using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ParkingChecker.OutputApi.Base.DataAccess;

namespace ParkingChecker.OutputApi.Base.Commands
{
    public interface IBaseFilterWithDefinitionsAndCountCommand<TDocument> where TDocument : IDocument
    {
        Task<FilterResponse<TDocument>> ExecuteAsync(string filterString, int skip, int top,
            Expression<Func<TDocument, object>> sortExpression, bool ascending);
    }
    public class BaseFilterWithDefinitionsAndCountCommand<TDocument> : IBaseFilterWithDefinitionsAndCountCommand<TDocument>
        where TDocument : IDocument
    {
        private readonly IBaseRepository<TDocument> _repository;

        public BaseFilterWithDefinitionsAndCountCommand(IBaseRepository<TDocument> repository)
        {
            _repository = repository;
        }
        public async Task<FilterResponse<TDocument>> ExecuteAsync(string filterString, int skip, int top, Expression<Func<TDocument, object>> sortExpression, bool ascending)
        {
            var response = new FilterResponse<TDocument>();
            response.Items = new List<TDocument>();
            var sortDefinitionBuilder = _repository.SortDefinitionBuilder;
            SortDefinition<TDocument> sortDefinition;

            if (filterString == null)
                filterString = "Deleted-Eq-false";
            else
                filterString += ";Deleted-Eq-false";

            if (ascending)
                sortDefinition = sortDefinitionBuilder.Ascending(sortExpression);
            else
                sortDefinition = sortDefinitionBuilder.Descending(sortExpression);

            var filterDefinitions = _repository.GetFilterDefinitionsFromString(filterString);

            var count = await _repository.GetCountAsync(filterDefinitions);

            if (skip > count && count >= top)
            {
                skip = (int)count - top;
            }
            if (skip > count && count < top)
            {
                skip = 0;
                top = (int)count;
            }
            if (skip < count && count < top)
            {
                top = (int)count - skip;
            }

            response.Items = await _repository.FilterWithSkipAsync(filterDefinitions, skip, top, sortDefinition);
            response.Count = count;
            return response;
        }
    }

    public class FilterResponse<TDocument>
    {
        public IEnumerable<TDocument> Items { get; set; }
        public long Count { get; set; }
    }
}
