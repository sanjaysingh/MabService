using MabService.Domain;
using MabService.Domain.Data;
using MabService.Domain.Shared;
using Microsoft.Azure.WebJobs.Host;

namespace MabService
{
    public class ServiceFactory
    {
        public static MockApiDefinitionService CreateMockApiDefinitionService(TraceWriter traceWriter)
        {
            var logger = new FunctionsTraceLogger(traceWriter);
            var settings = new MabAppSetting();
            var repo = new AzureTableMockApiRepository(settings.AzureStorageConnectionString, settings.MockApiDefinitionTableName);
            var manager = new MockApiManager(repo, logger);

            return new MockApiDefinitionService(manager, logger);
        }
    }
}
