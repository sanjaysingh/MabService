using MabService.Common;
using MabService.Shared;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using MabService.Shared.Exceptions;

namespace MabService
{
    /// <summary>
    /// Execute mock api service
    /// </summary>
    /// <seealso cref="MabService.Common.MockApiServiceBase" />
    public class ExecuteMockApiService : MockApiServiceBase 
    {
        private readonly ILanguageBindingFactory languageBindingFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteMockApiService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mockApiRepo">The mock API repo.</param>
        /// <param name="languageBindingFactory">The language binding factory.</param>
        public ExecuteMockApiService(ILogger logger, IMockApiRepository mockApiRepo, ILanguageBindingFactory languageBindingFactory) : base(logger, mockApiRepo)
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
            var routeData = req.GetRouteData();
            string collectionName = GetRouteValue(req, "collectionName");
            if (string.IsNullOrWhiteSpace(collectionName)) throw new ResourceNotFoundException();
            var collectionModel = await this.MockApiRepo.GetCollectionAsync(collectionName);
            var actualRoutePath = req.RequestUri.AbsolutePath.ToLower();
            foreach (var apiModel in collectionModel.MockApis)
            {
                var routeMatch = RouteUtil.MatchTemplate(actualRoutePath, apiModel.RouteTemplate); 
                if(routeMatch != null)
                {
                    var languageValidator = this.languageBindingFactory.CreateLanguageValidator(apiModel.Language);
                    languageValidator.Validate(apiModel.Body);
                    var apiLanguageBinding = this.languageBindingFactory.CreateLanguageBinding(apiModel.Language);
                    return apiLanguageBinding.Run(apiModel, req);
                }
            }

            throw new ResourceNotFoundException();
        }
    }
}
