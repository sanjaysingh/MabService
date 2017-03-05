using MabService.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabService.Domain
{
    /// <summary>
    /// Mock APIs manager
    /// </summary>
    public class MockApiManager : IMockApiManager
    {
        private readonly IMockApiRepository mockApiRepo;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockApiManager"/> class.
        /// </summary>
        /// <param name="mockApiRepo">The mock API repo.</param>
        /// <param name="logger">The logger.</param>
        public MockApiManager(IMockApiRepository mockApiRepo, ILogger logger)
        {
            this.mockApiRepo = mockApiRepo;
            this.logger = logger;
        }

        /// <summary>
        /// Creates the collection asynchronous.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns></returns>
        public async Task CreateCollectionAsync(string collectionName)
        {
            await this.mockApiRepo.CreateCollectionAsync(collectionName);
        }
    }
}
