using MabService.Common;
using MabService.Shared;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using MabService.Shared.Exceptions;
using System.Linq;
using System.Collections.Generic;

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
        [HttpGet]
        [HttpPut]
        [HttpDelete]
        protected override  async Task<HttpResponseMessage> ExecuteInternal(HttpRequestMessage req)
        {
            // get and validate collection name
            string collectionName = GetRouteValue(req, "collectionName");
            if (string.IsNullOrWhiteSpace(collectionName)) throw new ResourceNotFoundException();

            this.Logger.Info($"Executing api for collection {collectionName}");
            // retrieve collection
            var collectionModel = await this.MockApiRepo.GetCollectionAsync(collectionName);

            // find matching api definition, throw 404 if no match is found. Skip the collectionName in the route to match the template
            var absolutePath = req.RequestUri.AbsolutePath.Substring(req.RequestUri.AbsolutePath.ToLower().IndexOf(collectionName.ToLower()) + collectionName.Length);
            this.Logger.Info($"Absolute path:{absolutePath}");
            var actualRoutePath = RouteUtil.BuildRoute(RouteUtil.GetSegments(absolutePath));
            this.Logger.Info($"Actual Route Path:{actualRoutePath}");
            IDictionary<string, string> routeTemplateValuesMap = null;
            var apiModel = collectionModel.MockApis.FirstOrDefault(model => 
            {
                routeTemplateValuesMap = RouteUtil.MatchTemplate(actualRoutePath, model.RouteTemplate);
                return routeTemplateValuesMap != null;
            });
            if(apiModel == null)
            {
                throw new ResourceNotFoundException();
            }

            if(!HttpMethodMatch(req.Method, apiModel.Verb))
            {
                throw new MethodNotAllowedException();
            }

            // update route data for the API
            var routeData = req.GetRouteData();
            foreach (var templateKeyValue in routeTemplateValuesMap)
            {
                routeData.Values.Add(templateKeyValue.Key, templateKeyValue.Value);
            }

            // if a matching api definition is found, then validate the script
            this.languageBindingFactory.CreateLanguageValidator(apiModel.Language).Validate(apiModel.Body);

            // run the script now that all the validation has passed
            var apiLanguageBinding = this.languageBindingFactory.CreateLanguageBinding(apiModel.Language);
            return apiLanguageBinding.Run(apiModel, req);
        }

        /// <summary>
        /// HTTPs the method match.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="apiVerb">The API verb.</param>
        /// <returns>true if match false othwewise</returns>
        private static bool HttpMethodMatch(HttpMethod httpMethod, MockApiHttpVerb apiVerb)
        {
            switch (apiVerb)
            {
                case MockApiHttpVerb.Get:
                    return httpMethod == HttpMethod.Get;
                case MockApiHttpVerb.Post:
                    return httpMethod == HttpMethod.Post;
                case MockApiHttpVerb.Put:
                    return httpMethod == HttpMethod.Put;
                case MockApiHttpVerb.Delete:
                    return httpMethod == HttpMethod.Delete;
                default:
                    return false;
            }
        }
    }
}
