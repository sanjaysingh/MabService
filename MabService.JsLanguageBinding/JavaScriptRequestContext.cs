using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;

namespace MabService.JsLanguageBinding
{
    /// <summary>
    /// Request context for javascript API function
    /// </summary>
    public class JavaScriptRequestContext
    {
        private HttpRequestMessage hostRequestContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestContext" /> class.
        /// </summary>
        /// <param name="hostRequestContext">The host request context.</param>
        public JavaScriptRequestContext(HttpRequestMessage hostRequestContext)
        {
            this.hostRequestContext = hostRequestContext;
            this.header = ReadHeaders(hostRequestContext);
            this.query = ReadQueries(hostRequestContext);
            this.param = ReadRouteParams(hostRequestContext);
            this.clientIp = GetClientIp(hostRequestContext);
            if (hostRequestContext.Content != null)
            {
                this.content = hostRequestContext.Content.ReadAsAsync<ExpandoObject>().Result;
                this.rawContent = Encoding.UTF8.GetString(hostRequestContext.Content.ReadAsByteArrayAsync().Result);
            }
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public ReadOnlyDynamicObject<string> header { get; }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        public ReadOnlyDynamicObject<string> query { get; }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <value>
        /// The parameter.
        /// </value>
        public ReadOnlyDynamicObject<string> param { get; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <value>
        /// The payload.
        /// </value>
        public dynamic content { get; }

        /// <summary>
        /// Gets the rawpayload.
        /// </summary>
        /// <value>
        /// The rawpayload.
        /// </value>
        public string rawContent { get; }

        /// <summary>
        /// Gets the client ip.
        /// </summary>
        /// <value>
        /// The client ip.
        /// </value>
        public string clientIp { get; }

        /// <summary>
        /// Reads the headers.
        /// </summary>
        /// <param name="hostRequestContext">The host request context.</param>
        /// <returns>All the headers</returns>
        private static ReadOnlyDynamicObject<string> ReadHeaders(HttpRequestMessage hostRequestContext)
        {
            var headerDict = new Dictionary<string, string>();
            foreach (var currHeader in hostRequestContext.Headers)
            {
                headerDict.Add(currHeader.Key, string.Join(";", currHeader.Value));
            }

            return new ReadOnlyDynamicObject<string>(headerDict);
        }

        private static ReadOnlyDynamicObject<string> ReadQueries(HttpRequestMessage hostRequestContext)
        {
            var headerDict = new Dictionary<string, string>();
            foreach (var currQueryKeyValue in hostRequestContext.GetQueryNameValuePairs())
            {
                headerDict.Add(currQueryKeyValue.Key, currQueryKeyValue.Value);
            }

            return new ReadOnlyDynamicObject<string>(headerDict);
        }

        public static string GetClientIp(HttpRequestMessage request)
        {
            var forwardedForIp = request.Properties["HTTP_X_FORWARDED_FOR"];

            return string.IsNullOrWhiteSpace(forwardedForIp) ? HttpContext.Current.Request.UserHostAddress : forwardedForIp.Split(',')[0].Trim();
        }

        /// <summary>
        /// Reads the route parameters.
        /// </summary>
        /// <param name="hostRequestContext">The host request context.</param>
        /// <returns>Route parameters</returns>
        private static ReadOnlyDynamicObject<string> ReadRouteParams(HttpRequestMessage hostRequestContext)
        {
            var headerDict = new Dictionary<string, string>();
            foreach (var currRouteData in hostRequestContext.GetRouteData().Values)
            {
                if (!IsSystemRouteParam(currRouteData.Key))
                {
                    headerDict.Add(currRouteData.Key, currRouteData.Value.ToString());
                }
            }

            return new ReadOnlyDynamicObject<string>(headerDict);
        }

        /// <summary>
        /// Determines whether [is system route parameter] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if [is system route parameter] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsSystemRouteParam(string name)
        {
            return name.Equals("uri", System.StringComparison.OrdinalIgnoreCase) || name.Equals("controller", System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
