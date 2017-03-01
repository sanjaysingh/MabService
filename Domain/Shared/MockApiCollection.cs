using System.Collections.Generic;

namespace MabService.Domain.Shared
{
    /// <summary>
    /// Mock API collection
    /// </summary>
    public class MockApiCollection
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the mock apis.
        /// </summary>
        /// <value>
        /// The mock apis.
        /// </value>
        public List<MockApiModel> MockApis { get; set; } = new List<MockApiModel>();
    }
}
