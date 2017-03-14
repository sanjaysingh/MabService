using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Web.Http.Hosting;
using System.Web.Http;
using MabService.Data;
using MabService.Shared;
using MabService;
using System.Web.Http.Routing;
using MabService.Common;
using System.Collections.Generic;

namespace MabServiceTest
{
    [TestClass]
    public class ExecuteMockApiSpec
    {
        private InMemoryAzureTableMockApiRepository repo = new InMemoryAzureTableMockApiRepository();
        private ILogger logger = new NullLogger();
        private LanguageBindingFactory languageBindingFactory = new LanguageBindingFactory(new NullLogger());

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task ExecuteMockApiWithValidJsonResponseShouldBeOk()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "AddTwoNumber",
                Body = "function run(req, res) {" +
                "res.send( {result: parseInt(req.payload.num1,10) + parseInt(req.payload.num2,10)});" +
                
                "}",
                RouteTemplate = "math/addition",
                Verb = MockApiHttpVerb.Post
            };
            await AddMockApi(mockApi);
            var routeValues = new HttpRouteValueDictionary() { { "collectionName", "sanjaysingh" }};

            var response = await RunMockApi("http://localhost/sanjaysingh/math/addition", new { num1=4, num2=5}, mockApi.RouteTemplate, HttpMethod.Post, routeValues);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            double responsePayload =  (await response.Content.ReadAsAsync<dynamic>()).result;

            responsePayload.Should().Be(9);
        }
        
        private async Task<HttpResponseMessage> AddMockApi(object requestBody, HttpMethod httpMethod = null)
        {
            AddMockApiService service = new AddMockApiService(logger, repo, languageBindingFactory);

            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod ?? HttpMethod.Post, "http://localhost/collection/sanjaysingh");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var route = new HttpRoute("collection/{collectionName}");
            var routeValues = new HttpRouteValueDictionary() { { "collectionName", "sanjaysingh" } };
            var routeData = new HttpRouteData(route, routeValues);
            requestMessage.SetRouteData(routeData);

            return await service.Execute(requestMessage);
        }

        private async Task<HttpResponseMessage> RunMockApi(string url, object requestBody,
                                                                    string routeTemplate,
                                                                    HttpMethod httpMethod,
                                                                    HttpRouteValueDictionary routeValues)
        {
            ExecuteMockApiService service = new ExecuteMockApiService(logger, repo, languageBindingFactory);

            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod ?? HttpMethod.Post, url);
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var route = new HttpRoute(routeTemplate);
            var routeData = new HttpRouteData(route, routeValues);
            requestMessage.SetRouteData(routeData);

            return await service.Execute(requestMessage);
        }
    }
}
