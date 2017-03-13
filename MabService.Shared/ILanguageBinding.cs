using System.Net.Http;

namespace MabService.Shared
{
    /// <summary>
    /// Language binding 
    /// </summary>
    public interface ILanguageBinding
    {
        /// <summary>
        /// Runs the specified API to run.
        /// </summary>
        /// <param name="apiToRun">The API to run.</param>
        /// <param name="requestMeta">The request meta.</param>
        /// <returns>
        /// the response of method execution
        /// </returns>
        HttpResponseMessage Run(MockApiModel apiToRun, HttpRequestMessage requestMeta);
    }
}
