using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MabService.Shared;
using System;
using MabService.Shared.Exceptions;

namespace MabService.Data
{
    /// <summary>
    /// An in memory implementation azure table mock api repository
    /// </summary>
    /// <seealso cref="MabService.Domain.Shared.IMockApiRepository" />
    public class InMemoryAzureTableMockApiRepository : IMockApiRepository
    {
        private List<MockApiEntity> table = new List<MockApiEntity>();

        /// <summary>
        /// Adds the mock API.
        /// </summary>
        /// <param name="mockApi">The mock API.</param>
        /// <returns>Mock API definition</returns>
        public Task<MockApiModel> AddMockApiAsync(MockApiModel mockApi, string collectionName)
        {
            var collectionEntity = table.SingleOrDefault(x => x.RowKey == collectionName.ToLower() && x.PartitionKey == collectionName.ToLower());

            if (collectionEntity == null)
                throw new ResourceNotFoundException();

            var mockApiEntity = new MockApiEntity(mockApi.Name, collectionEntity.Name)
            {
                RouteTemplate = mockApi.RouteTemplate,
                Verb = mockApi.Verb.ToString(),
                Body = mockApi.Body,
                Language = mockApi.Language.ToString()
            };

            if (table.Any(x => x.RowKey == mockApiEntity.RowKey))
                throw new ResourceConflictException();

            table.Add(mockApiEntity);

            return Task.FromResult(mockApi);
        }

        /// <summary>
        /// Collections the exists.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        /// <returns>true if exists, false otherwise</returns>
        public Task<bool> CheckCollectionExistsAsync(string collectionName)
        {
            collectionName = collectionName.ToLower();
            return Task.FromResult(this.table.Exists(row => row.PartitionKey == collectionName && row.RowKey == collectionName));
        }

        /// <summary>
        /// Creates the collection.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>task object</returns>
        public Task CreateCollectionAsync(string collectionName)
        {
            var collectionEntity = new MockApiEntity(collectionName, collectionName);
            if (table.Any(x => x.PartitionKey == collectionEntity.PartitionKey))
                throw new ResourceConflictException();
            table.Add(collectionEntity);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>collection of mock APIs</returns>
        public Task<MockApiCollectionModel> GetCollectionAsync(string collectionName)
        {
            collectionName = collectionName.ToLower();
            var mockApiModels = new List<MockApiModel>();
            var entities = table.Where(row => row.PartitionKey == collectionName);

            if (!entities.Any())
            {
                throw new CollectionNotFoundException();
            }

            var actualCollectionName = collectionName;
            foreach (MockApiEntity entity in entities)
            {
                if (!entity.RowKey.Equals(collectionName))
                {
                    mockApiModels.Add(new MockApiModel(entity.Name,
                                                        entity.RouteTemplate,
                                                        entity.Body,
                                                        entity.Verb.ToEnum<MockApiHttpVerb>(),
                                                        entity.Language.ToEnum<MockApiLanguage>()));
                }
                else
                {
                    actualCollectionName = entity.CollectionName;
                }
            }

            return Task.FromResult(new MockApiCollectionModel(actualCollectionName, mockApiModels));
        }
    }
}
