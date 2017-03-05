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
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost/collection");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var requestBody = new { collectionName = "MyCollection" };
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var repo = new InMemoryAzureTableMockApiRepository();
            var logger = new NullLogger();
            CreateCollectionService service = new CreateCollectionService(logger, repo);
            var response = await service.Execute(requestMessage);

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }
    }
}
