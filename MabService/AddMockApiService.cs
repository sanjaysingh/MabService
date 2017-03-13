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
        private readonly ILanguageBindingFactory languageBindingFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddMockApiService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mockApiRepo">The mock API repo.</param>
        /// <param name="languageBindingFactory">The language binding factory.</param>
        public AddMockApiService(ILogger logger, IMockApiRepository mockApiRepo, ILanguageBindingFactory languageBindingFactory) : base(logger, mockApiRepo)
        {
            this.languageBindingFactory = languageBindingFactory;
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

            // read and validate mock api to be added
            var mockAPI = await req.Content.ReadAsAsync<MockApiResourceModel>();
            Validator.ValidateMockApiResource(mockAPI, languageBindingFactory);

            // add the mock api
            var response = await this.MockApiRepo.AddMockApiAsync(mockAPI.ToDomainModel(), collectionName);
            return req.CreateResponse(HttpStatusCode.OK, MockApiResourceModel.FromDomainModel(response));
        }
    }
}
