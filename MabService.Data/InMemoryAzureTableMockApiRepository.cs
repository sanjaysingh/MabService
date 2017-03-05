using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MabService.Shared;

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
            var mockApiEntity = new MockApiEntity(mockApi.Name, collectionName)
            {
                RouteTemplate = mockApi.RouteTemplate,
                Verb = mockApi.Verb,
                Body = mockApi.Body
            };
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
            var mockApiModels = new List<MockApiModel>();
            foreach (MockApiEntity entity in table.Where(row => row.PartitionKey == collectionName))
            {
                mockApiModels.Add(new MockApiModel()
                {
                    Name = entity.RowKey,
                    Body = entity.Body,
                    Verb = entity.Verb,
                    RouteTemplate = entity.RouteTemplate
                });
            }
            return Task.FromResult(new MockApiCollectionModel()
            {
                Name = collectionName,
                MockApis = mockApiModels
            });
        }
    }
}
