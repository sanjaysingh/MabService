using MabService.Shared;
using System.Threading.Tasks;
using System.Net.Http;

namespace MabService.Common
{
    /// <summary>
    /// Base class for all servies
    /// </summary>
    public abstract class MockApiServiceBase : IMockApiService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockApiServiceBase"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected MockApiServiceBase(ILogger logger)
        {
            this.Logger = logger;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ILogger Logger
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes the specified req.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns>http response message</returns>
        public async Task<HttpResponseMessage> Execute(HttpRequestMessage req)
        {
            return await this.ExecuteInternal(req);
        }

        /// <summary>
        /// Overrides the actual implementation of the service
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        protected abstract Task<HttpResponseMessage> ExecuteInternal(HttpRequestMessage req);
    }
}
