using MabService.Common;
using MabService.Data;
using MabService.Shared;
using Microsoft.Azure.WebJobs.Host;

namespace MabService.FunctionsHelper
{
    /// <summary>
    /// Service Factory
    /// </summary>
    public class ServiceFactory
    {
        /// <summary>
        /// Creates the collection service.
        /// </summary>
        /// <param name="traceWriter">The trace writer.</param>
        /// <returns>CreateCollectionService</returns>
        public static CreateCollectionService CreateCollectionService(TraceWriter traceWriter)
        {
            var logger = new FunctionsTraceLogger(traceWriter);
            var repo = new AzureTableMockApiRepository(ServiceLocator.AppSetting.AzureStorageConnectionString, ServiceLocator.AppSetting.MockApiDefinitionTableName);

            return new CreateCollectionService(logger, repo);
        }

        /// <summary>
        /// Creates the get collection service.
        /// </summary>
        /// <param name="traceWriter">The trace writer.</param>
        /// <returns>GetCollectionService</returns>
        public static GetCollectionService CreateGetCollectionService(TraceWriter traceWriter)
        {
            var logger = new FunctionsTraceLogger(traceWriter);
            var repo = new AzureTableMockApiRepository(ServiceLocator.AppSetting.AzureStorageConnectionString, ServiceLocator.AppSetting.MockApiDefinitionTableName);

            return new GetCollectionService(logger, repo);
        }

        /// <summary>
        /// Creates the get collection reference service.
        /// </summary>
        /// <param name="traceWriter">The trace writer.</param>
        /// <returns>collection reference service</returns>
        public static GetCollectionReferenceService CreateGetCollectionReferenceService(TraceWriter traceWriter)
        {
            var logger = new FunctionsTraceLogger(traceWriter);
            var repo = new AzureTableMockApiRepository(ServiceLocator.AppSetting.AzureStorageConnectionString, ServiceLocator.AppSetting.MockApiDefinitionTableName);

            return new GetCollectionReferenceService(logger, repo);
        }

        /// <summary>
        /// Creates the add mock API service.
        /// </summary>
        /// <param name="traceWriter">The trace writer.</param>
        /// <returns>AddMockApiService</returns>
        public static AddMockApiService CreateAddMockApiService(TraceWriter traceWriter)
        {
            var logger = new FunctionsTraceLogger(traceWriter);
            var repo = new AzureTableMockApiRepository(ServiceLocator.AppSetting.AzureStorageConnectionString, ServiceLocator.AppSetting.MockApiDefinitionTableName);
            var languageBindingFactory = new LanguageBindingFactory(logger);
            return new AddMockApiService(logger, repo, languageBindingFactory);
        }
    }
}
