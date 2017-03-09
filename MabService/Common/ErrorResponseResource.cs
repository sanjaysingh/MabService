using MabService.Shared.Exceptions;
using System;
using System.Collections.Generic;

namespace MabService.Common
{
    /// <summary>
    /// Error response Model
    /// </summary>
    public class ErrorResponseResource
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="ErrorResponseResource"/> class from being created.
        /// </summary>
        private ErrorResponseResource()
        {
        }

        /// <summary>
        /// Froms the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static ErrorResponseResource From(ExceptionBase ex) => From(ex.GetType().Name, ex.Errors);

        /// <summary>
        /// Froms the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public static ErrorResponseResource From(string id, string error) => From(id, new string[] { error });

        /// <summary>
        /// Froms the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static ErrorResponseResource From(string id, IEnumerable<string> errors) => new ErrorResponseResource() { Id = id, Errors = errors };

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
