using System.Net.Http;
using System.Threading.Tasks;

namespace MabService.Common
{
    /// <summary>
    /// Mock Api service contract
    /// </summary>
    public interface IMockApiService
    {
        Task<HttpResponseMessage> Execute(HttpRequestMessage req);
    }
}
