using System;

namespace MabService.Shared.Exceptions
{
    /// <summary>
    /// Disallowed http method exception
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class DisallowedHttpMethodException : ExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisallowedHttpMethodException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DisallowedHttpMethodException(string message) : base(message)
        {
        }
    }
}
