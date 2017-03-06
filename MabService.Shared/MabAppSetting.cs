using System.Configuration;

namespace MabService.Shared
{
    /// <summary>
    /// Mab App Settings implementation
    /// </summary>
    /// <seealso cref="MabService.Domain.Shared.IMabAppSetting" />
    public class MabAppSetting : IMabAppSetting
    {
        private const string DefaultMockApiDefinitionTableName = "MockApiDefinition";

        /// <summary>
        /// Gets the azure storage connection string.
        /// </summary>
        /// <value>
        /// The azure storage connection string.
        /// </value>
        public string AzureStorageConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["AzureStorageConnectionString"];
            }
        }

        /// <summary>
        /// Gets the name of the mock API definition table.
        /// </summary>
        /// <value>
        /// The name of the mock API definition table.
        /// </value>
        public string MockApiDefinitionTableName
        {
            get
            {
                return ConfigurationManager.AppSettings["MockApiDefinitionTableName"]?? DefaultMockApiDefinitionTableName;
            }
        }
    }
}
