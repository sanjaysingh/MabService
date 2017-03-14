using MabService.Shared;
using System.Net.Http;

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

        /// <summary>
        /// Runs the specified API to run.
        /// </summary>
        /// <param name="apiToRun">The API to run.</param>
        /// <param name="hostRequestContext">The host request context.</param>
        /// <returns>HttpResponseMessage</returns>
        public HttpResponseMessage Run(MockApiModel apiToRun, HttpRequestMessage hostRequestContext)
        {
            var requestContext = new JavaScriptRequestContext(hostRequestContext);
            var responseContext = new JavaScriptResponseContext();
            JavaScriptRuntime.Run(apiToRun.Body, requestContext, responseContext);
            return responseContext.CreateHostResponse(hostRequestContext);
        }
    }
}
