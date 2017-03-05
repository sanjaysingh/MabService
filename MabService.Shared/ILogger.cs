using System;

namespace MabService.Shared
{
    /// <summary>
    /// Logget contract
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs specified message as information
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        void Info(string message, string source = null);

        /// <summary>
        /// Logs specified message as warning
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        void Warning(string message, string source = null);

        /// <summary>
        /// Logs specified message as error
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="source">The source.</param>
        void Error(string message, Exception ex = null, string source = null);
    }
}
