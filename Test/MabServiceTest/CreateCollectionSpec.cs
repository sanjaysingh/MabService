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
        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task CreateCollectionWithValidCollectionNameShouldReturnOk()
        {
            var response = await Execute(new { collectionName = "MyCollection" });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        [TestCategory("Acceptance Test")]
        public async Task CreateCollectionWithInvalidHttpMethodShouldReturnNotFound()
        {
            var response = await Execute(new { collectionName = "MyCollection" }, HttpMethod.Put);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task CreateCollectionWithSpecialCharsInNameShouldReturnBadRequest()
        {
            var response = await Execute(new { collectionName = "My#Collection" }, HttpMethod.Post);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task CreateCollectionWith35CharsInNameShouldReturnBadRequest()
        {
            var response = await Execute(new { collectionName = "".PadLeft(35,'A') }, HttpMethod.Post);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private static async Task<HttpResponseMessage> Execute(object requestBody, HttpMethod httpMethod = null)
        {
            var repo = new InMemoryAzureTableMockApiRepository();
            var logger = new NullLogger();
            CreateCollectionService service = new CreateCollectionService(logger, repo);

            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod ?? HttpMethod.Post, "http://localhost/collection");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            return await service.Execute(requestMessage);
        }
    }
}
