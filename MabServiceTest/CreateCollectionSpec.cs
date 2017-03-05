using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using MabService;
using MabService.Domain.Data;
using MabService.Domain;
using MabService.Domain.Shared;
using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Web.Http.Hosting;
using System.Web.Http;

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
            var manager = new MockApiManager(repo, logger);
            MockApiDefinitionService service = new MockApiDefinitionService(manager, logger);
            var response = await service.CreateCollectionAsync(requestMessage);

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }
    }
}
