using System.Collections.Generic;

namespace MabService.Common
{
    /// <summary>
    /// Error response Model
    /// </summary>
    public class ErrorResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseModel"/> class.
        /// </summary>
        public ErrorResponseModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseModel"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="error">The error.</param>
        public ErrorResponseModel(string id, string error) : this(id, new string[] { error})
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseModel"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="errors">The errors.</param>
        public ErrorResponseModel(string id, IEnumerable<string> errors)
        {
            this.Id = id;
            this.Errors = errors;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IEnumerable<string> Errors { get; private set; }
    }
}
