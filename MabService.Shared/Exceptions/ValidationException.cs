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
        public ValidationException(string message, string error) : base(message, error)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errors">The errors.</param>
        public ValidationException(string message, IEnumerable<string> errors): base(message, errors)
        {
        }
    }
}
