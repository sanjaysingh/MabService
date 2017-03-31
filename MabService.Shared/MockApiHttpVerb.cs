using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MabService.Shared
{
    /// <summary>
    /// Mock Api Verb
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MockApiHttpVerb
    {
        None = 0,
        Get = 1,
        Post = 2,
        Put = 3,
        Delete = 4
    }
}
