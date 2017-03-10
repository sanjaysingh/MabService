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
        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithValidDataShouldReturnOk()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "AddTwoNumber",
                Body = "function(num1, num2) {return num1+num2;}",
                RouteTemplate = "math/addition/{num1}/{num2}",
                Verb = MockApiHttpVerb.Post
            };
            var response = await Execute(mockApi);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithUnspecifiedApiNameShouldReturnBadRequest()
        {
            var mockApi = new MockApiResourceModel()
            {
                Body = "function(num1, num2) {return num1+num2;}",
                RouteTemplate = "math/addition",
                Verb = MockApiHttpVerb.Post
            };
            var response = await Execute(mockApi);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithBigApiNameShouldReturnBadRequest()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "".PadLeft(Constants.MaxNameLength+1, 'A'),
                Body = "function(num1, num2) {return num1+num2;}",
                RouteTemplate = "math/addition",
                Verb = MockApiHttpVerb.Post
            };
            var response = await Execute(mockApi);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithUnspecifiedRouteTemplateShouldReturnBadRequest()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "addnumber",
                Body = "function(num1, num2) {return num1+num2;}",
                Verb = MockApiHttpVerb.Post
            };
            var response = await Execute(mockApi);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithBigRouteTemplateShouldReturnBadRequest()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "addnumber",
                Body = "function(num1, num2) {return num1+num2;}",
                RouteTemplate = "".PadLeft(Constants.MaxApiTemplateLength+1, 'A'),
                Verb = MockApiHttpVerb.Post
            };
            var response = await Execute(mockApi);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithUnspecifiedBodyShouldReturnBadRequest()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "".PadLeft(25, 'A'),
                RouteTemplate = "".PadLeft(20, 'A'),
                Verb = MockApiHttpVerb.Post
            };
            var response = await Execute(mockApi);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithBigBodyDefinitionShouldReturnBadRequest()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "".PadLeft(25, 'A'),
                RouteTemplate = "".PadLeft(20, 'A'),
                Body = "function(){return sdaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaddsaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaasaddddddddddddddddddddsaaaaaaaaaaaaaaaaaaaadddddddddddddddddddddddaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasad2111111111111111ddddddddddddddddddddddddddddd1111111111111111aaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa211111111111111dddddddddddddddddddddddddd1111111111111111111aaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa211111ddddddddddddddddddddddddddddddddddddd11111111111111111aaaaaaa" +
                "sadddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddaaaaaaaa;}",
                Verb = MockApiHttpVerb.Post
            };
            var response = await Execute(mockApi);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithIncorrectHttpVerbShouldReturnNotFound()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "".PadLeft(25, 'A'),
                RouteTemplate = "".PadLeft(20, 'A'),
                Body = "function(){return \"a\";}",
                Verb = MockApiHttpVerb.Post
            };
            var response = await Execute(mockApi, HttpMethod.Put);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithInvalidCharsInRouteTemplateShouldReturnBadRequest()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "".PadLeft(25, 'A'),
                RouteTemplate = "<script>alert 'hi'</script>",
                Body = "function(){return \"a\";}",
                Verb = MockApiHttpVerb.Post
            };
            var response = await Execute(mockApi, HttpMethod.Post);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        private static async Task<HttpResponseMessage> Execute(object requestBody, HttpMethod httpMethod = null)
        {
            var repo = new InMemoryAzureTableMockApiRepository();
            var logger = new NullLogger();
            AddMockApiService service = new AddMockApiService(logger, repo);

            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod ?? HttpMethod.Post, "http://localhost/collection/sanjaysingh");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var route = new HttpRoute("collection/{collectionName}");
            var routeValues = new HttpRouteValueDictionary() { { "collectionName", "sanjaysingh" } };
            var routeData = new HttpRouteData(route, routeValues);
            requestMessage.SetRouteData(routeData);

            return await service.Execute(requestMessage);
        }
    }
}
