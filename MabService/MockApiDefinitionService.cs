using MabService.Domain.Shared;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MabService
{
    /// <summary>
    /// Mock api definition related service
    /// </summary>
    public class MockApiDefinitionService
    {
        private readonly IMockApiManager mockApiManager;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockApiDefinitionService"/> class.
        /// </summary>
        /// <param name="mockApiManager">The mock API manager.</param>
        /// <param name="logger">The logger.</param>
        public MockApiDefinitionService(IMockApiManager mockApiManager, ILogger logger)
        {
            this.mockApiManager = mockApiManager;
            this.logger = logger;
        }

        public async Task<HttpResponseMessage> CreateCollectionAsync(HttpRequestMessage req)
        {
            dynamic body = await req.Content.ReadAsAsync<object>();
            string collectionName = body.collectionName;
            await this.mockApiManager.CreateCollectionAsync(collectionName);
            return req.CreateResponse(HttpStatusCode.OK, ResourceStrings.CollectionCreated(collectionName));
        }
    }
}
