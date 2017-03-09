using System.Collections.Generic;
using System.Linq;

namespace MabService.Shared
{
    /// <summary>
    /// Mock API collection
    /// </summary>
    public class MockApiCollectionModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockApiCollectionModel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="mockApis">The mock apis.</param>
        public MockApiCollectionModel(string name, IEnumerable<MockApiModel> mockApis)
        {
            this.Name = name;
            this.MockApis = mockApis.ToList();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the mock apis.
        /// </summary>
        /// <value>
        /// The mock apis.
        /// </value>
        public IEnumerable<MockApiModel> MockApis { get; } = new List<MockApiModel>();
    }
}
