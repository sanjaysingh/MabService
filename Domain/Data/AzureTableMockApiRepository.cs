using System;
using MabService.Domain.Shared;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MabService.Domain.Data
{
    /// <summary>
    /// A document db implementation for mock api repository
    /// </summary>
    /// <seealso cref="MabService.Domain.Shared.IMockApiRepository" />
    public class AzureTableMockApiRepository : IMockApiRepository
    {
        private readonly string storageConnectionString;
        private readonly string tableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTableMockApiRepository"/> class.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        /// <param name="tableName">Name of the table.</param>
        public AzureTableMockApiRepository(string storageConnectionString, string tableName)
        {
            this.storageConnectionString = storageConnectionString;
            this.tableName = tableName;
        }

        /// <summary>
        /// Adds the mock API.
        /// </summary>
        /// <param name="mockApi">The mock API.</param>
        /// <returns>Mock API definition</returns>
        public async Task<MockApiModel> AddMockApiAsync(MockApiModel mockApi, string collectionName)
        {
            var table = await this.GetTableReferenceAsync();

            var mockApiEntity = new MockApiEntity(mockApi.Name, collectionName)
            {
                RouteTemplate = mockApi.RouteTemplate,
                Verb = mockApi.Verb,
                Body = mockApi.Body
            };
            var insertOperation = TableOperation.Insert(mockApiEntity);
            await table.ExecuteAsync(insertOperation);

            return mockApi;
        }

        /// <summary>
        /// Collections the exists.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        /// <returns>true if exists, false otherwise</returns>
        public async Task<bool> CheckCollectionExistsAsync(string collectionName)
        {
            var table = await this.GetTableReferenceAsync();

            var retrieveOperation = TableOperation.Retrieve<MockApiEntity>(collectionName, collectionName);
            var retrievedResult = await table.ExecuteAsync(retrieveOperation);

            return retrievedResult.Result != null;
        }

        /// <summary>
        /// Creates the collection.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>task object</returns>
        public async Task CreateCollectionAsync(string collectionName)
        {
            var table = await this.GetTableReferenceAsync();
            var collectionEntity = new MockApiEntity(collectionName, collectionName);
            var insertOperation = TableOperation.Insert(collectionEntity);
            await table.ExecuteAsync(insertOperation);
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>collection of mock APIs</returns>
        public async Task<MockApiCollectionModel> GetCollectionAsync(string collectionName)
        {
            var table = await this.GetTableReferenceAsync();
            var retrieveOperation = TableOperation.Retrieve<MockApiEntity>(collectionName, collectionName);
            var retrievedResult = await table.ExecuteAsync(retrieveOperation);
            var query = new TableQuery<MockApiEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, collectionName));

            var mockApiModels = new List<MockApiModel>();
            foreach (MockApiEntity entity in table.ExecuteQuery(query))
            {
                mockApiModels.Add(new MockApiModel()
                {
                    Name = entity.RowKey,
                    Body = entity.Body,
                    Verb = entity.Verb,
                    RouteTemplate = entity.RouteTemplate
                });
            }
            return new MockApiCollectionModel()
            {
                Name = collectionName,
                MockApis = mockApiModels
            };
        }

        /// <summary>
        /// Gets the table reference.
        /// </summary>
        /// <returns></returns>
        private async Task<CloudTable> GetTableReferenceAsync()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
