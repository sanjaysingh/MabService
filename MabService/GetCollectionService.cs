using MabService.Common;
using MabService.Shared;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using System.Net;

namespace MabService
{
    /// <summary>
    /// Add mock api service
    /// </summary>
    /// <seealso cref="MabService.Common.MockApiServiceBase" />
    public class GetCollectionService : MockApiServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCollectionService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mockApiRepo">The mock API repo.</param>
        public GetCollectionService(ILogger logger, IMockApiRepository mockApiRepo) : base(logger, mockApiRepo)
        {
        }

        /// <summary>
        /// Overrides the actual implementation of the service
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        protected override  async Task<HttpResponseMessage> ExecuteInternal(HttpRequestMessage req)
        {
            // read and validate collection name
            var routeData = req.GetRouteData();
            string collectionName = string.Empty;
            if(routeData.Values != null && routeData.Values.ContainsKey("collectionName"))
            {
                collectionName = routeData.Values["collectionName"].ToString();
            }
            Validator.ValidateCollectionName(collectionName);

            // get and return collection
            var response = await this.MockApiRepo.GetCollectionAsync(collectionName);
            return req.CreateResponse(HttpStatusCode.OK, MockApiCollectionResourceModel.FromDomainModel(response));
        }
    }
}
