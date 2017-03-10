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
using MabService.Common;
using System.Linq;
using System.Web.Http.Routing;

namespace MabServiceTest
{
    [TestClass]
    public class GetCollectionSpec
    {
        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionWithOneApiShouldReturnOneApi()
        {
            IMockApiRepository repo = new InMemoryAzureTableMockApiRepository();
            string collectionName = "myCollection";
            await CreateCollection(repo, collectionName);
            var mockApi = new MockApiResourceModel() {
                Name = "AddTwoNumbers",
                RouteTemplate = "addition/{num1}/{num2}",
                Body = "function(num1, num2) { return num1+num2;}",
                Verb = MockApiHttpVerb.Get
            };
            await AddMockApi(repo, collectionName, mockApi);

            var collection = await GetCollectionModel(repo, collectionName);

            collection.Name.Should().Be(collectionName);
            collection.MockApis.Count.Should().Be(1);
            collection.MockApis.First().ShouldBeEquivalentTo(mockApi);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionWithTwoApiShouldReturnTwoApis()
        {
            IMockApiRepository repo = new InMemoryAzureTableMockApiRepository();
            string collectionName = "myCollection";
            await CreateCollection(repo, collectionName);
            var mockApi1 = new MockApiResourceModel()
            {
                Name = "AddTwoNumbers",
                RouteTemplate = "addition/{num1}/{num2}",
                Body = "function(num1, num2) { return num1+num2;}",
                Verb = MockApiHttpVerb.Get
            };
            await AddMockApi(repo, collectionName, mockApi1);
            var mockApi2 = new MockApiResourceModel()
            {
                Name = "AddTwoNumbers2",
                RouteTemplate = "addition2/{num1}/{num2}",
                Body = "function(num1, num2) { return num1+num2;}",
                Verb = MockApiHttpVerb.Get
            };
            await AddMockApi(repo, collectionName, mockApi2);

            var collection = await GetCollectionModel(repo, collectionName);

            collection.Name.Should().Be(collectionName);
            collection.MockApis.Count.Should().Be(2);
            collection.MockApis.First(x => x.Name == mockApi1.Name).ShouldBeEquivalentTo(mockApi1);
            collection.MockApis.First(x => x.Name == mockApi2.Name).ShouldBeEquivalentTo(mockApi2);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionWithNoApiShouldReturnNoApis()
        {
            IMockApiRepository repo = new InMemoryAzureTableMockApiRepository();
            string collectionName = "myCollection";
            await CreateCollection(repo, collectionName);

            var collection = await GetCollectionModel(repo, collectionName);

            collection.Name.Should().Be(collectionName);
            collection.MockApis.Count.Should().Be(0);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionWithInvalidCollectionNameShouldReturnBadRequest()
        {
            IMockApiRepository repo = new InMemoryAzureTableMockApiRepository();

            var response = await GetCollectionRawResponse(repo, "asdd#");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionWithUnavailableCollectionNameShouldReturnNotFound()
        {
            IMockApiRepository repo = new InMemoryAzureTableMockApiRepository();

            var response = await GetCollectionRawResponse(repo, "abcd");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private static async Task<HttpResponseMessage> CreateCollection(IMockApiRepository repo, string collectionName)
        {
            var logger = new NullLogger();
            CreateCollectionService service = new CreateCollectionService(logger, repo);

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost/collection");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(new { collectionName = collectionName }), Encoding.UTF8, "application/json");

            return await service.Execute(requestMessage);
        }
        private static async Task<HttpResponseMessage> AddMockApi(IMockApiRepository repo, string collectionName, MockApiResourceModel mockApi)
        {
            var logger = new NullLogger();
            AddMockApiService service = new AddMockApiService(logger, repo);

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost/collection/"+collectionName);
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var route = new HttpRoute("collection/{collectionName}");
            var routeValues = new HttpRouteValueDictionary() { { "collectionName", collectionName } };
            var routeData = new HttpRouteData(route, routeValues);
            requestMessage.SetRouteData(routeData);
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(mockApi), Encoding.UTF8, "application/json");

            return await service.Execute(requestMessage);
        }

        private static async Task<MockApiCollectionResourceModel> GetCollectionModel(IMockApiRepository repo, string collectionName)
        {
            return await (await GetCollectionRawResponse(repo, collectionName)).Content.ReadAsAsync<MockApiCollectionResourceModel>();
        }

        private static async Task<HttpResponseMessage> GetCollectionRawResponse(IMockApiRepository repo, string collectionName)
        {
            var logger = new NullLogger();
            GetCollectionService service = new GetCollectionService(logger, repo);

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost/collection/" + collectionName);
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var route = new HttpRoute("collection/{collectionName}");
            var routeValues = new HttpRouteValueDictionary() { { "collectionName", collectionName } };
            var routeData = new HttpRouteData(route, routeValues);
            requestMessage.SetRouteData(routeData);

            return await service.Execute(requestMessage);
        }
    }
}
