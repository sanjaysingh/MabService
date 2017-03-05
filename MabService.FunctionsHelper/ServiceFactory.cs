using MabService.Data;
using MabService.Shared;
using Microsoft.Azure.WebJobs.Host;

namespace MabService.FunctionsHelper
{
    public class ServiceFactory
    {
        public static CreateCollectionService CreateCollectionService(TraceWriter traceWriter)
        {
            var logger = new FunctionsTraceLogger(traceWriter);
            var settings = new MabAppSetting();
            var repo = new AzureTableMockApiRepository(settings.AzureStorageConnectionString, settings.MockApiDefinitionTableName);

            return new CreateCollectionService(logger, repo);
        }
    }
}
