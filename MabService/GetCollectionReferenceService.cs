using System.Net.Http;
using System.Threading.Tasks;
using MabService.Common;
using MabService.Shared;
using System.Net;
using System.Web.Http;
using MabService.Shared.Exceptions;

namespace MabService
{
    /// <summary>
    /// Collection reference service
    /// </summary>
    /// <seealso cref="MabService.Common.MockApiServiceBase" />
    /// <seealso cref="MabService.Common.IMockApiService" />
    public class GetCollectionReferenceService : MockApiServiceBase, IMockApiService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCollectionReferenceService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public GetCollectionReferenceService(ILogger logger, IMockApiRepository mockApiRepo) : base(logger, mockApiRepo)
        {
        }

        /// <summary>
        /// Overrides the actual implementation of the service
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        [HttpGet]
        protected override async Task<HttpResponseMessage> ExecuteInternal(HttpRequestMessage req)
        {
            // read and validate collection name
            var routeData = req.GetRouteData();
            string collectionName = string.Empty;
            if (routeData.Values != null && routeData.Values.ContainsKey("collectionName"))
            {
                collectionName = routeData.Values["collectionName"].ToString();
            }

            Logger.Info($"Collection name received = {collectionName}");
            bool exists =  await this.MockApiRepo.CheckCollectionExistsAsync(collectionName);

            Logger.Info($"Collection exists = {exists}");

            if (!exists) throw new CollectionNotFoundException();

            Logger.Info($"Returning collection reference = {collectionName}");

            return req.CreateResponse(HttpStatusCode.OK, new CollectionReferenceResource() { Name = collectionName});
        }
    }
}
