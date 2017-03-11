using MabService.Common;
using MabService.Shared;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Linq;

namespace MabService
{
    /// <summary>
    /// Execute mock api service
    /// </summary>
    /// <seealso cref="MabService.Common.MockApiServiceBase" />
    public class ExecuteMockApiService : MockApiServiceBase 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteMockApiService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mockApiRepo">The mock API repo.</param>
        public ExecuteMockApiService(ILogger logger, IMockApiRepository mockApiRepo) : base(logger, mockApiRepo)
        {
        }

        /// <summary>
        /// Overrides the actual implementation of the service
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        protected override  async Task<HttpResponseMessage> ExecuteInternal(HttpRequestMessage req)
        {
            // read and validate collection name
            //req.GetRouteData().Values.Values.First().ToString()

            string data = string.Empty;
            foreach (var keyValue in req.GetRouteData().Values)
            {
                data += string.Format("Key={0}, Value={1}\n", keyValue.Key, keyValue.Value);
            }
            return req.CreateResponse(HttpStatusCode.OK, "");
        }
    }
}
