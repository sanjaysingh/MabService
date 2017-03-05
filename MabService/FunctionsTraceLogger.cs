using MabService.Domain.Shared;
using Microsoft.Azure.WebJobs.Host;
using System;

namespace MabService
{
    /// <summary>
    /// Functions trace logger implementation
    /// </summary>
    /// <seealso cref="MabService.Domain.Shared.ILogger" />
    public class FunctionsTraceLogger : ILogger
    {
        private readonly TraceWriter traceWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionsTraceLogger"/> class.
        /// </summary>
        /// <param name="traceWriter">The trace writer.</param>
        public FunctionsTraceLogger(TraceWriter traceWriter)
        {
            this.traceWriter = traceWriter;
        }

        /// <summary>
        /// Logs specified message as error
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="source">The source.</param>
        public void Error(string message, Exception ex = null, string source = null)
        {
            this.traceWriter.Error(message, ex, source);
        }

        /// <summary>
        /// Logs specified message as information
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        public void Info(string message, string source = null)
        {
            this.traceWriter.Info(message, source);
        }

        /// <summary>
        /// Logs specified message as warning
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        public void Warning(string message, string source = null)
        {
            this.traceWriter.Warning(message, source);
        }
    }
}
