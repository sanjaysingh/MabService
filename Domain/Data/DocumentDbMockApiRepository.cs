using System;
using MabService.Domain.Shared;

namespace MabService.Domain.Data
{
    /// <summary>
    /// A document db implementation for mock api repository
    /// </summary>
    /// <seealso cref="MabService.Domain.Shared.IMockApiRepository" />
    public class DocumentDbMockApiRepository : IMockApiRepository
    {
        /// <summary>
        /// Adds the mock API.
        /// </summary>
        /// <param name="mockApi">The mock API.</param>
        /// <returns>Mock API definition</returns>
        public MockApiModel AddMockApi(MockApiModel mockApi)
        {
            return null;
        }

        /// <summary>
        /// Collections the exists.
        /// </summary>
        /// <param name="collectionId">The collection identifier.</param>
        /// <returns>true if exists, false otherwise</returns>
        public bool CollectionExists(string collectionId)
        {
            return false;
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>collection of mock APIs</returns>
        public MockApiCollection GetCollection(string id)
        {
            return null;
        }
    }
}
