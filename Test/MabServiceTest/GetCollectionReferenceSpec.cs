using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Hosting;
using System.Web.Http;
using MabService.Data;
using MabService.Shared;
using MabService;
using MabService.Common;
using System.Web.Http.Routing;

namespace MabServiceTest
{
    [TestClass]
    public class GetCollectionReferenceSpec
    {
        private InMemoryAzureTableMockApiRepository repo = new InMemoryAzureTableMockApiRepository();
        private GetCollectionReferenceService getCollectionReferenceService;
        private const string collectionName = "MyCollection";

        [TestInitialize]
        public void TestInitialize()
        {
            this.repo.CreateCollectionAsync(collectionName).Wait();
            var logger = new NullLogger();
            this.getCollectionReferenceService = new GetCollectionReferenceService(logger, this.repo);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionWithOneApiShouldReturnCollection()
        {
            var name = "AddTwoNumbers";
            var routeTemplate = "addition/{num1}/{num2}";
            var body = "function(num1, num2) { return num1+num2;}";
            var verb = MockApiHttpVerb.Get;

            var mockApi = new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript) ;
            await this.repo.AddMockApiAsync(mockApi, collectionName);

            var collection = await GetCollectionReferenceModel();

            collection.Name.Should().Be(collectionName);
        }
        
        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionReferenceWithNoApiShouldReturnNoApis()
        {
            var collection = await GetCollectionReferenceModel();

            collection.Name.Should().Be(collectionName);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionReferenceWithInvalidHttpMethodShouldReturnMethodNotAllowed()
        {
            var response = await GetCollectionRawResponse(httpMethod: HttpMethod.Put);
            response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionReferenceWithInvalidCollectionNameShouldReturnNotFound()
        {
            var response = await GetCollectionRawResponse("asdd#");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task GetCollectionReferenceWithUnavailableCollectionNameShouldReturnNotFound()
        {
            var response = await GetCollectionRawResponse("abcd");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<CollectionReferenceResource> GetCollectionReferenceModel(string collectionName = collectionName)
        {
            return await (await GetCollectionRawResponse(collectionName)).Content.ReadAsAsync<CollectionReferenceResource>();
        }

        private async Task<HttpResponseMessage> GetCollectionRawResponse(string collectionName = collectionName, HttpMethod httpMethod = null)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod ?? HttpMethod.Get, "http://localhost/collectionreference/" + collectionName);
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var route = new HttpRoute("collectionreference/{collectionName}");
            var routeValues = new HttpRouteValueDictionary() { { "collectionName", collectionName } };
            var routeData = new HttpRouteData(route, routeValues);
            requestMessage.SetRouteData(routeData);

            return await getCollectionReferenceService.Execute(requestMessage);
        }
    }
}
