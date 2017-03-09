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

namespace MabServiceTest
{
    [TestClass]
    public class AddMockApiSpec
    {
        [TestMethod]
        public async Task AddMockApiWithValidDataShouldReturnOk()
        {
            var repo = new InMemoryAzureTableMockApiRepository();
            var logger = new NullLogger();
            AddMockApiService service = new AddMockApiService(logger, repo);
            var mockApi = new MockApiResourceModel()
            {
                Name = "AddTwoNumber",
                Body = "function(num1, num2) {return num1+num2;}",
                RouteTemplate = "math/addition",
                Verb = MockApiHttpVerb.Post
            };
            var response = await service.Execute(CreateRequestMessage(mockApi));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private HttpRequestMessage CreateRequestMessage(object requestBody, HttpMethod httpMethod = null)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod ?? HttpMethod.Post, "http://localhost/collection/sanjaysingh");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var route = new HttpRoute("collection/{collectionName}");
            var routeValues = new HttpRouteValueDictionary() { { "collectionName", "sanjaysingh" } };
            var routeData = new HttpRouteData(route, routeValues);
            requestMessage.SetRouteData(routeData);
            return requestMessage;
        }
    }
}
