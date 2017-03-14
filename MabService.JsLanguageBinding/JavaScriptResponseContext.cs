using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace MabService.JsLanguageBinding
{
    /// <summary>
    /// Javascript response context
    /// </summary>
    public class JavaScriptResponseContext
    {
        private IDictionary<string, string> headers = new Dictionary<string, string>();
        private object body = null;
        private int statusCode = 200;

        /// <summary>
        /// Headers the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>JavaScriptResponseContext</returns>
        public JavaScriptResponseContext header(string key, string value)
        {
            this.headers[key] = value;
            return this;
        }

        /// <summary>
        /// Statuses the specified status code.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>JavaScriptResponseContext</returns>
        public JavaScriptResponseContext status(int statusCode)
        {
            this.statusCode = statusCode;
            return this;
        }

        /// <summary>
        /// Sends the specified body.
        /// </summary>
        /// <param name="body">The body.</param>
        public void send(dynamic body)
        {
            this.body = body;
        }

        /// <summary>
        /// Jsons the specified body.
        /// </summary>
        /// <param name="body">The body.</param>
        public void json(dynamic body)
        {
            this.body = body;
            this.headers["Content-Type"] = "application/json";
        }

        /// <summary>
        /// Determines whether this instance is json.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is json; otherwise, <c>false</c>.
        /// </returns>
        private bool IsJson()
        {
            return this.headers.ContainsKey("Content-Type") && this.headers["Content-Type"].ToLower().Contains("json");
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <returns>Content-Type</returns>
        private string GetContentType()
        {
            return this.headers.ContainsKey("Content-Type")? this.headers["Content-Type"] : null;
        }

        /// <summary>
        /// Gets the media type formatter.
        /// </summary>
        /// <returns>Media type formatter</returns>
        private MediaTypeFormatter GetMediaTypeFormatter()
        {
            return new JsonMediaTypeFormatter();
        }

        /// <summary>
        /// Creates the host response.
        /// </summary>
        /// <param name="requestMessage">The request message.</param>
        /// <returns>HttpResponseMessage</returns>
        internal HttpResponseMessage CreateHostResponse(HttpRequestMessage requestMessage)
        {
            var response = requestMessage.CreateResponse();
            
            // status code
            response.StatusCode = (HttpStatusCode) this.statusCode;

            // set body if specified
            if(this.body != null)
            {
                response.Content = new ObjectContent(this.body.GetType(), this.body, GetMediaTypeFormatter());
            }

            // set headers
            foreach(var headerKeyValue in this.headers)
            {
                response.Headers.Add(headerKeyValue.Key, headerKeyValue.Value);
            }

            return response;
        }
    }
}
