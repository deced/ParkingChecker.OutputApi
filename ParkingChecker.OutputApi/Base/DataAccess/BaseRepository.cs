using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ParkingChecker.OutputApi.Configuration;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;

namespace ParkingChecker.OutputApi.Base.DataAccess
{
    public class BaseRepository<TDocument> : IBaseRepository<TDocument> where TDocument : IDocument
    {
        public FilterDefinitionBuilder<TDocument> FilterDefinitionBuilder { get; }
        public SortDefinitionBuilder<TDocument> SortDefinitionBuilder { get; }

        private readonly IMongoCollection<TDocument> _collection;

        public BaseRepository(IDatabaseConfiguration settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
            FilterDefinitionBuilder = new FilterDefinitionBuilder<TDocument>();
            SortDefinitionBuilder = new SortDefinitionBuilder<TDocument>();
        }

        private string GetCollectionName(Type documentType)
        {
            return ((CollectionNameAttribute) documentType.GetCustomAttributes(
                    typeof(CollectionNameAttribute),
                    true)
                .FirstOrDefault())?.Name;
        }

        public async Task<IEnumerable<TDocument>> FilterWithSkipAsync(IEnumerable<FilterDefinition<TDocument>> filterDefinitions, int skip, int count, SortDefinition<TDocument> sortDefinition = null)
        {
            var findOptions = new FindOptions<TDocument>
            {
                Skip = skip,
                Limit = count,
                Sort = sortDefinition
            };

            var documents = await _collection.FindAsync(FilterDefinitionBuilder.And(filterDefinitions), findOptions);
            return documents.ToEnumerable();
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual Task<IEnumerable<TDocument>> FilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => { return _collection.Find(filterExpression).ToEnumerable(); });
        }

        public virtual Task<IEnumerable<TDocument>> SortFilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, object>> sortObject)
        {
            return Task.Run(() => { return _collection.Find(filterExpression).SortBy(sortObject).ToEnumerable(); });
        }

        public virtual Task<IEnumerable<TDocument>> SortLimitFilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, object>> sortObject,
            int skip, int top)
        {
            return Task.Run(() =>
            {
                return _collection.Find(filterExpression).SortBy(sortObject).Skip(skip).Limit(top).ToEnumerable();
            });
        }

        public virtual Task<IEnumerable<TDocument>> SortByDescendingLimitFilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, object>> sortObject,
            int skip, int top)
        {
            return Task.Run(() =>
            {
                return _collection.Find(filterExpression).SortByDescending(sortObject).Skip(skip).Limit(top)
                    .ToEnumerable();
            });
        }

        public virtual Task<IEnumerable<TDocument>> LimitFilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression, int take)
        {
            return Task.Run(() => { return _collection.Find(filterExpression).Limit(take).ToEnumerable(); });
        }

        public async Task<IEnumerable<TDocument>> FindManySortAndUpdateAsync(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, object>> sortObject, UpdateDefinition<TDocument> updateDefinition,
            int count)
        {
            var documents = _collection.Find(filterExpression).SortBy(sortObject).Limit(count).ToEnumerable();
            var filter = Builders<TDocument>.Filter.In(x => x.Id, documents.Select(x => x.Id));
            await _collection.UpdateManyAsync(filter, updateDefinition);

            return documents;
        }

        public virtual Task<long> FilterCountByAsync(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.CountAsync(filterExpression);
        }

        public List<FilterDefinition<TDocument>> GetFilterDefinitionsFromString(string filterString)
        {
            var filterDefinitions = new List<FilterDefinition<TDocument>> { FilterDefinitionBuilder.Empty };
            if (string.IsNullOrWhiteSpace(filterString)) return filterDefinitions;

            var filters = filterString.Split(";");
            foreach (var filter in filters)
            {
                var filterParts = filter.Split("-");
                var property = filterParts[0];
                var action = filterParts[1].ToLower();
                var value = filterParts[2];

                if (action == "eq")
                {
                    var values = value.Split("||");
                    List<FilterDefinition<TDocument>> _filters = new List<FilterDefinition<TDocument>>();
                    foreach (var item in values)
                    {
                        _filters.Add(FilterDefinitionBuilder.Eq(property, item));
                    }
                    filterDefinitions.Add(FilterDefinitionBuilder.Or(_filters));
                }
                else if (action == "contains")
                {
                    var regexString = @$"\w*(?i){value}";
                    filterDefinitions.Add(FilterDefinitionBuilder.Regex(property, regexString));
                }
                else if (action == "in")
                {
                    var values = value.Split(";");
                    filterDefinitions.Add(FilterDefinitionBuilder.In(property, values));
                }
                else
                {
                    throw new InvalidOperationException($"Invalid action: {action}");
                }
            }

            return filterDefinitions;
        }

        public virtual Task<IEnumerable<TDocument>> GetAllAsync()
        {
            return Task.Run(() => { return _collection.Find(_ => true).ToEnumerable(); });
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public async Task<long> GetCountAsync(IEnumerable<FilterDefinition<TDocument>> filterDefinitions)
        {
            return await _collection.CountDocumentsAsync(FilterDefinitionBuilder.And(filterDefinitions));
        }
    }
}