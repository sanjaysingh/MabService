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

namespace MabServiceTest
{
    [TestClass]
    public class CreateCollectionSpec
    {
        private CreateCollectionService createCollectionService;
        private const string collectionName = "MyCollection";

        [TestInitialize]
        public void TestInitialize()
        {
            this.createCollectionService = new CreateCollectionService(new NullLogger(), new InMemoryAzureTableMockApiRepository());
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task CreateCollectionWithValidCollectionNameShouldReturnOk()
        {
            var response = await CreateCollection();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task CreateCollectionWithInvalidHttpMethodShouldReturnNotFound()
        {
            var response = await CreateCollection( httpMethod: HttpMethod.Put);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task CreateCollectionWithSpecialCharsInNameShouldReturnBadRequest()
        {
            var response = await CreateCollection("My#Collection", HttpMethod.Post);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task CreateCollectionWith35CharsInNameShouldReturnBadRequest()
        {
            var response = await CreateCollection("".PadLeft(35, 'A'), HttpMethod.Post);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private async Task<HttpResponseMessage> CreateCollection(string collectionName = collectionName, HttpMethod httpMethod = null)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod ?? HttpMethod.Post, "http://localhost/collection");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(new { collectionName = collectionName }), Encoding.UTF8, "application/json");

            return await this.createCollectionService.Execute(requestMessage);
        }
    }
}
