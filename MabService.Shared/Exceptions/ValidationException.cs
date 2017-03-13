using System;
using System.Collections.Generic;

namespace MabService.Shared.Exceptions
{
    /// <summary>
    /// Validation Exception
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ValidationException : ExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="error">The error.</param>
        public ValidationException(string error) : this(error, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="error">The error.</param>
        public ValidationException(string error, Exception innerException) : this ("Validation error", new string[] { error}, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errors">The errors.</param>
        public ValidationException(IEnumerable<string> errors): this("Validation error", errors, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errors">The errors.</param>
        /// <param name="innerException">The inner exception.</param>
        public ValidationException(string message, IEnumerable<string> errors,Exception innerException) : base(message, errors, innerException)
        {
        }
    }
}
