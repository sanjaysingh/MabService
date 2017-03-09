namespace MabService.Shared
{
    /// <summary>
    /// Mock Api Model
    /// </summary>
    public class MockApiModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockApiModel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="routeTemplate">The route template.</param>
        /// <param name="body">The body.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="language">The language.</param>
        public MockApiModel(string name,
                            string routeTemplate,
                            string body,
                            MockApiHttpVerb verb,
                            MockApiLanguage language)
        {

            this.Name = name;
            this.RouteTemplate = routeTemplate;
            this.Body = body;
            this.Verb = verb;
            this.Language = language;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the route template.
        /// </summary>
        /// <value>
        /// The route template.
        /// </value>
        public string RouteTemplate { get; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; }

        /// <summary>
        /// Gets or sets the verb.
        /// </summary>
        /// <value>
        /// The verb.
        /// </value>
        public MockApiHttpVerb Verb { get; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public MockApiLanguage Language { get; }
    }
}
