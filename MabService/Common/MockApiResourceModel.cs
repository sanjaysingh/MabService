using MabService.Shared;
namespace MabService.Common
{
    /// <summary>
    /// MockApiResourceModel
    /// </summary>
    public class MockApiResourceModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Name { get; set; }

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
        public MockApiHttpVerb Verb { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public MockApiLanguage Language { get; set; }

        /// <summary>
        /// To the domain model.
        /// </summary>
        /// <returns></returns>
        public MockApiModel ToDomainModel() => new MockApiModel(this.Name, this.RouteTemplate, this.Body, this.Verb, this.Language);

        /// <summary>
        /// Froms the domain model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MockApiResourceModel FromDomainModel(MockApiModel model) => new MockApiResourceModel()
        {
            Name = model.Name,
            Body = model.Body,
            Language = model.Language,
            RouteTemplate = model.RouteTemplate,
            Verb = model.Verb
        };
    }
}
