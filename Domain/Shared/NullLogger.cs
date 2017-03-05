using System;

namespace MabService.Domain.Shared
{
    /// <summary>
    /// A do nothing logger implementation.
    /// </summary>
    /// <seealso cref="MabService.Domain.Shared.ILogger" />
    public class NullLogger : ILogger
    {
        /// <summary>
        /// Logs specified message as error
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="source">The source.</param>
        public void Error(string message, Exception ex = null, string source = null)
        {
        }

        /// <summary>
        /// Logs specified message as information
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        public void Info(string message, string source = null)
        {
        }

        /// <summary>
        /// Logs specified message as warning
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        public void Warning(string message, string source = null)
        {
        }
    }
}
