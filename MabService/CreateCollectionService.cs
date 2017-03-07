using System;
using System.Net.Http;
using System.Threading.Tasks;
using MabService.Common;
using MabService.Shared;
using System.Net;
using System.Web.Http;

namespace MabService
{
    /// <summary>
    /// Create a collection service
    /// </summary>
    /// <seealso cref="MabService.Common.MockApiServiceBase" />
    /// <seealso cref="MabService.Common.IMockApiService" />
    public class CreateCollectionService : MockApiServiceBase, IMockApiService
    {
        private readonly IMockApiRepository mockApiRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCollectionService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CreateCollectionService(ILogger logger, IMockApiRepository mockApiRepo) : base(logger)
        {
            this.mockApiRepo = mockApiRepo;
        }

        /// <summary>
        /// Overrides the actual implementation of the service
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        [HttpPost]
        protected override async Task<HttpResponseMessage> ExecuteInternal(HttpRequestMessage req)
        {
            dynamic body = await req.Content.ReadAsAsync<object>();
            string collectionName = body.collectionName;
            await this.mockApiRepo.CreateCollectionAsync(collectionName);
            return req.CreateResponse(HttpStatusCode.OK, ResourceStrings.CollectionCreated(collectionName));
        }
    }
}
