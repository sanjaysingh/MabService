﻿using System.Threading.Tasks;

namespace MabService.Shared
{
    /// <summary>
    /// A mock API data repository contract
    /// </summary>
    public interface IMockApiRepository
    {
        /// <summary>
        /// Checks the collection exists asynchronous.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>true if collection exists, false otherwise</returns>
        Task<bool> CheckCollectionExistsAsync(string collectionName);

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>api collection</returns>
        Task<MockApiCollectionModel> GetCollectionAsync(string collectionName);

        /// <summary>
        /// Creates the collection.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        Task CreateCollectionAsync(string collectionName);

        /// <summary>
        /// Adds the mock API.
        /// </summary>
        /// <param name="mockApi">The mock API.</param>
        /// <returns>The added mock API definition</returns>
        Task<MockApiModel> AddMockApiAsync(MockApiModel mockApi, string collectionName);
    }
}
