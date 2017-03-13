using MabService.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MabService.JsLanguageBinding
{
    /// <summary>
    /// Java script language binding implementation
    /// </summary>
    /// <seealso cref="MabService.Shared.ILanguageBinding" />
    public class JavaScriptLanguageBinding : ILanguageBinding
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptLanguageBinding"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public JavaScriptLanguageBinding(ILogger logger)
        {
            this.logger = logger;
        }
        public HttpResponseMessage Run(MockApiModel apiToRun, HttpRequestMessage hostRequestContext)
        {
            var requestContext = new JavaScriptRequestContext(hostRequestContext);

            return hostRequestContext.CreateResponse(HttpStatusCode.OK);
        }
    }
}
