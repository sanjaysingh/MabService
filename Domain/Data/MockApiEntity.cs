using MabService.Domain.Shared;
using Microsoft.WindowsAzure.Storage.Table;

namespace MabService.Domain.Data
{
    /// <summary>
    /// Mock Api entity
    /// </summary>
    /// <seealso cref="Microsoft.WindowsAzure.Storage.Table.TableEntity" />
    public class MockApiEntity : TableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockApiEntity"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="collectionName">Name of the collection.</param>
        public MockApiEntity(string name, string collectionName)
        {
            this.PartitionKey = collectionName;
            this.RowKey = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockApiEntity"/> class.
        /// </summary>
        public MockApiEntity()
        {
        }

        /// <summary>
        /// Gets or sets the route template.
        /// </summary>
        /// <value>
        /// The route template.
        /// </value>
        public string RouteTemplate { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the verb.
        /// </summary>
        /// <value>
        /// The verb.
        /// </value>
        public string Verb { get; set; }
    }
}
