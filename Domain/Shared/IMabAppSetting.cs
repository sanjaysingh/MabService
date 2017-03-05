namespace MabService.Domain.Shared
{
    /// <summary>
    /// Mab app settings
    /// </summary>
    public interface IMabAppSetting
    {
        /// <summary>
        /// Gets the azure storage connection string.
        /// </summary>
        /// <value>
        /// The azure storage connection string.
        /// </value>
        string AzureStorageConnectionString { get; }

        /// <summary>
        /// Gets the name of the mock API definition table.
        /// </summary>
        /// <value>
        /// The name of the mock API definition table.
        /// </value>
        string MockApiDefinitionTableName { get; }
    }
}
