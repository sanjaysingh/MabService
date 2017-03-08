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
        public async Task CreateCollectionWithValidCollectionNameShouldReturnOk()
        {
            var repo = new InMemoryAzureTableMockApiRepository();
            var logger = new NullLogger();
            CreateCollectionService service = new CreateCollectionService(logger, repo);
            var response = await service.Execute(CreateRequestMessage(new { collectionName = "MyCollection" }));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task CreateCollectionWithInvalidHttpMethodShouldReturnNotFound()
        {
            var repo = new InMemoryAzureTableMockApiRepository();
            var logger = new NullLogger();
            CreateCollectionService service = new CreateCollectionService(logger, repo);
            var response = await service.Execute(CreateRequestMessage(new { collectionName = "MyCollection" }, HttpMethod.Put));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task CreateCollectionWithSpecialCharsInNameShouldReturnBadRequest()
        {
            var repo = new InMemoryAzureTableMockApiRepository();
            var logger = new NullLogger();
            CreateCollectionService service = new CreateCollectionService(logger, repo);
            var response = await service.Execute(CreateRequestMessage(new { collectionName = "My#Collection" }, HttpMethod.Post));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task CreateCollectionWith25CharsInNameShouldReturnBadRequest()
        {
            var repo = new InMemoryAzureTableMockApiRepository();
            var logger = new NullLogger();
            CreateCollectionService service = new CreateCollectionService(logger, repo);
            var response = await service.Execute(CreateRequestMessage(new { collectionName = "".PadLeft(25,'A') }, HttpMethod.Post));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private HttpRequestMessage CreateRequestMessage(object requestBody, HttpMethod httpMethod = null)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod ?? HttpMethod.Post, "http://localhost/collection");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            return requestMessage;
        }
    }
}
