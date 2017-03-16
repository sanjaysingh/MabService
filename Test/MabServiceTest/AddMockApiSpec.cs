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
        private InMemoryAzureTableMockApiRepository repo = new InMemoryAzureTableMockApiRepository();
        private AddMockApiService addApiservice;
        private const string collectionName = "MyCollection";

        [TestInitialize]
        public void TestInitialize()
        {
            this.repo.CreateCollectionAsync(collectionName).Wait();
            var logger = new NullLogger();
            var languageBindingFactory = new LanguageBindingFactory(logger);
            this.addApiservice = new AddMockApiService(logger, this.repo, languageBindingFactory);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithValidDataShouldReturnOk()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "AddTwoNumber",
                Body = "function run(req, res) {return req.num1+req.num2;}",
                RouteTemplate = "math/addition/{num1}/{num2}",
                Verb = MockApiHttpVerb.Post
            };
            var response = await AddApi(mockApi);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithUnspecifiedApiNameShouldReturnBadRequest()
        {
            var mockApi = new MockApiResourceModel()
            {
                Body = "function run(req, res) {return req.num1+req.num2;}",
                RouteTemplate = "math/addition",
                Verb = MockApiHttpVerb.Post
            };
            var response = await AddApi(mockApi);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithBigApiNameShouldReturnBadRequest()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "".PadLeft(Constants.MaxNameLength+1, 'A'),
                Body = "function run(req, res) {return req.num1+req.num2;}",
                RouteTemplate = "math/addition",
                Verb = MockApiHttpVerb.Post
            };
            var response = await AddApi(mockApi);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithUnspecifiedRouteTemplateShouldReturnBadRequest()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "addnumber",
                Body = "function run(req, res) {return req.num1+req.num2;}",
                Verb = MockApiHttpVerb.Post
            };
            var response = await AddApi(mockApi);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task AddMockApiWithBigRouteTemplateShouldReturnBadRequest()
        {
            var mockApi = new MockApiResourceModel()
            {
                Name = "addnumber",
                Body = "function run(req, res) {return req.num1+req.num2;}",
                RouteTemplate = "".PadLeft(Constants.MaxApiTemplateLength+1, 'A'),
                Verb = MockApiHttpVerb.Post
            };
            var response = await AddApi(mockApi);

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
            var response = await AddApi(mockApi);

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
            var response = await AddApi(mockApi);

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
                Body = "function run(req, res) {return req.num1+req.num2;}",
                Verb = MockApiHttpVerb.Post
            };
            var response = await AddApi(mockApi, HttpMethod.Put);

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
            var response = await AddApi(mockApi, HttpMethod.Post);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private async Task<HttpResponseMessage> AddApi(object requestBody, HttpMethod httpMethod = null)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod ?? HttpMethod.Post, "http://localhost/sanjaysingh");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var route = new HttpRoute("collection/{collectionName}");
            var routeValues = new HttpRouteValueDictionary() { { "collectionName", collectionName } };
            var routeData = new HttpRouteData(route, routeValues);
            requestMessage.SetRouteData(routeData);

            return await addApiservice.Execute(requestMessage);
        }
    }
}
