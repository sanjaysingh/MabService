using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MabService.Shared
{
    /// <summary>
    /// ApiFunctionLanguage
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MockApiLanguage
    {
        JavaScript = 0
    }
}
