using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MabService.Domain.Data;
using FluentAssertions;
using System.Threading.Tasks;

namespace MabService.DomainTest.Data
{
    [Ignore]
    [TestClass]
    public class AzureTableMockApiRepositoryTest
    {
        private string storageConnectionString = Environment.GetEnvironmentVariable("MockApisStorageAccount", EnvironmentVariableTarget.User);

        [TestMethod]
        public async Task CreateCollectionAsync_DidNotExist_ShouldBeAdded()
        {
            var mockApiRepo = new AzureTableMockApiRepository(storageConnectionString, "mockApisTest");
            string collectionName = "SanjaySingh2";
            await mockApiRepo.CreateCollectionAsync(collectionName);
            var collectionModel = await mockApiRepo.GetCollectionAsync(collectionName);

            collectionModel.Name.ShouldBeEquivalentTo(collectionName);
        }
    }
}
