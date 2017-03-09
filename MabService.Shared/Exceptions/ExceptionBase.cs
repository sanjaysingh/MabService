using System;
using System.Collections.Generic;
using System.Linq;

namespace MabService.Shared.Exceptions
{
    public class ExceptionBase : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected ExceptionBase(string message):base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="error">The error.</param>
        protected ExceptionBase(string message, string error):this(message, new string[] { error})
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errors">The errors.</param>
        protected ExceptionBase(string message, IEnumerable<string> errors):base(message)
        {
            this.Errors = errors;
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IEnumerable<string> Errors { get; } = Enumerable.Empty<string>();
    }
}
