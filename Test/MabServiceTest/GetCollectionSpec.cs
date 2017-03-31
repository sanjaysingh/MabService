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
        private InMemoryAzureTableMockApiRepository repo = new InMemoryAzureTableMockApiRepository();
        private GetCollectionService getCollectionService;
        private const string collectionName = "MyCollection";

        [TestInitialize]
        public void TestInitialize()
        {
            this.repo.CreateCollectionAsync(collectionName).Wait();
            var logger = new NullLogger();
            this.getCollectionService = new GetCollectionService(logger, this.repo);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionWithOneApiShouldReturnOneApi()
        {
            var name = "AddTwoNumbers";
            var routeTemplate = "addition/{num1}/{num2}";
            var body = "function(num1, num2) { return num1+num2;}";
            var verb = MockApiHttpVerb.Get;

            var mockApi = new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript);
            await this.repo.AddMockApiAsync(mockApi, collectionName);

            var collection = await GetCollectionModel();

            collection.Name.Should().Be(collectionName);
            collection.MockApis.Count.Should().Be(1);
            collection.MockApis.First().ShouldBeEquivalentTo(mockApi);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionWithTwoApiShouldReturnTwoApis()
        {
            var mockApi1 = new MockApiModel("AddTwoNumbers1", "addition1/{num1}/{num2}", "function run(num1, num2) { return num1+num2;}", MockApiHttpVerb.Get, MockApiLanguage.JavaScript);
            var mockApi2 = new MockApiModel("AddTwoNumbers2", "addition2/{num1}/{num2}", "function run(num1, num2) { return num1+num2;}", MockApiHttpVerb.Get, MockApiLanguage.JavaScript);
            await this.repo.AddMockApiAsync(mockApi1, collectionName);
            await this.repo.AddMockApiAsync(mockApi2, collectionName);

            var collection = await GetCollectionModel();

            collection.Name.Should().Be(collectionName);
            collection.MockApis.Count.Should().Be(2);
            collection.MockApis.First(x => x.Name == mockApi1.Name).ShouldBeEquivalentTo(mockApi1);
            collection.MockApis.First(x => x.Name == mockApi2.Name).ShouldBeEquivalentTo(mockApi2);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionWithNoApiShouldReturnNoApis()
        {
            var collection = await GetCollectionModel();

            collection.Name.Should().Be(collectionName);
            collection.MockApis.Count.Should().Be(0);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionWithInvalidHttpMethodShouldReturnMethodNotAllowed()
        {
            var response = await GetCollectionRawResponse(httpMethod: HttpMethod.Put);
            response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionWithInvalidCollectionNameShouldReturnNotFound()
        {
            var response = await GetCollectionRawResponse("asdd#");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionWithUnavailableCollectionNameShouldReturnNotFound()
        {
            var response = await GetCollectionRawResponse("abcd");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<MockApiCollectionResourceModel> GetCollectionModel(string collectionName = collectionName)
        {
            return await (await GetCollectionRawResponse(collectionName)).Content.ReadAsAsync<MockApiCollectionResourceModel>();
        }

        private async Task<HttpResponseMessage> GetCollectionRawResponse(string collectionName = collectionName, HttpMethod httpMethod = null)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod ?? HttpMethod.Get, "http://localhost/collection/" + collectionName);
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var route = new HttpRoute("collection/{collectionName}");
            var routeValues = new HttpRouteValueDictionary() { { "collectionName", collectionName } };
            var routeData = new HttpRouteData(route, routeValues);
            requestMessage.SetRouteData(routeData);
            return await getCollectionService.Execute(requestMessage);
        }
    }
}
