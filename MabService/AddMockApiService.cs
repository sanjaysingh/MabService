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
    public class AddMockApiService : MockApiServiceBase
    {
        private readonly IMockApiRepository mockApiRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddMockApiService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mockApiRepo">The mock API repo.</param>
        public AddMockApiService(ILogger logger, IMockApiRepository mockApiRepo) : base(logger)
        {
            this.mockApiRepo = mockApiRepo;
        }

        /// <summary>
        /// Overrides the actual implementation of the service
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
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

            // read and validate mock api to be added
            var mockAPI = await req.Content.ReadAsAsync<MockApiResourceModel>();
            Validator.ValidateMockApiResource(mockAPI);

            // add the mock api
            var response = await this.mockApiRepo.AddMockApiAsync(mockAPI.ToDomainModel(), collectionName);
            return req.CreateResponse(HttpStatusCode.OK, MockApiResourceModel.FromDomainModel(response));
        }
    }
}
