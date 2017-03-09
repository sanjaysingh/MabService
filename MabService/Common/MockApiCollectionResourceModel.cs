using MabService.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MabService.Common
{
    /// <summary>
    /// MockApiCollectionResourceModel
    /// </summary>
    public class MockApiCollectionResourceModel
    {
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
        public List<MockApiResourceModel> MockApis { get; set; } = new List<MockApiResourceModel>();

        /// <summary>
        /// Froms the domain model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MockApiCollectionResourceModel FromDomainModel(MockApiCollectionModel model) => new MockApiCollectionResourceModel()
        {
            Name = model.Name,
            MockApis = model.MockApis.Select(x => MockApiResourceModel.FromDomainModel(x)).ToList()
        };
    }
}
