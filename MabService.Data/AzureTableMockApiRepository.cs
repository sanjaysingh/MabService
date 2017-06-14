using System.Threading.Tasks;
using System.Collections.Generic;
using MabService.Shared;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System.Linq;
using MabService.Shared.Exceptions;
using System;

namespace MabService.Data
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
            var collectionEntity = await RetrieveCollectionEntity(collectionName);
            if (collectionEntity == null)
            {
                throw new CollectionNotFoundException();
            }

            var table = await this.GetTableReferenceAsync();

            var mockApiEntity = new MockApiEntity(mockApi.Name, collectionEntity.CollectionName)
            {
                RouteTemplate = mockApi.RouteTemplate,
                Verb = mockApi.Verb.ToString(),
                Body = mockApi.Body,
                Language = mockApi.Language.ToString()
            };
            try
            {
                var insertOperation = TableOperation.Insert(mockApiEntity);
                await table.ExecuteAsync(insertOperation);
            }
            catch(StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == 409)
                {
                    throw new ResourceConflictException();
                }
                throw;
            }

            return mockApi;
        }

        /// <summary>
        /// Checks the collection exists asynchronous.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>true of collection exists, false otherwise</returns>
        public async Task<bool> CheckCollectionExistsAsync(string collectionName)
        {
            return await RetrieveCollectionEntity(collectionName) != null;
        }

        /// <summary>
        /// Creates the collection.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>task object</returns>
        public async Task CreateCollectionAsync(string collectionName)
        {
            try
            {
                var table = await this.GetTableReferenceAsync();
                var collectionEntity = new MockApiEntity(collectionName, collectionName);
                var insertOperation = TableOperation.Insert(collectionEntity);
                await table.ExecuteAsync(insertOperation);
            }
            catch(StorageException ex)
            {
                if(ex.RequestInformation.HttpStatusCode == 409)
                {
                    throw new ResourceConflictException();
                }
                throw;
            }
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>collection of mock APIs</returns>
        public async Task<MockApiCollectionModel> GetCollectionAsync(string collectionName)
        {
            var table = await this.GetTableReferenceAsync();
            var query = new TableQuery<MockApiEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, collectionName));

            var mockApiModels = new List<MockApiModel>();
            var entities = table.ExecuteQuery(query);
            if (!entities.Any())
            {
                throw new CollectionNotFoundException();
            }
            var actualCollectionName = collectionName;
            foreach (MockApiEntity entity in table.ExecuteQuery(query))
            {
                // There will always be at least one row with collection name as the entity, ignore that and return rest
                if (!entity.RowKey.Equals(collectionName, StringComparison.OrdinalIgnoreCase))
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
            return new MockApiCollectionModel(actualCollectionName, mockApiModels);
        }

        /// <summary>
        /// Gets the collection entity.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>collection entity</returns>
        private async Task<MockApiEntity> RetrieveCollectionEntity(string collectionName)
        {
            collectionName = collectionName.ToLower();
            var table = await this.GetTableReferenceAsync();
            var retrieveOperation = TableOperation.Retrieve<MockApiEntity>(collectionName, collectionName);
            var result = (await table.ExecuteAsync(retrieveOperation));

            return result.Result as MockApiEntity;
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
