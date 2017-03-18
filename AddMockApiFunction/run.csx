#load "..\common.csx"

using System.Net;
using MabService;
using MabService.Shared;
using MabService.FunctionsHelper;
using Jint;
using Jint.Native;
public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string collectionName, TraceWriter log)
{
    log.Info($"AddMockApiService is processing a request. RequestUri={req.RequestUri}");
    var service = ServiceFactory.CreateAddMockApiService(log);
    log.Info($"Adding a mockapi to collection: {collectionName}");
    req.GetRouteData().Values.Add("collectionName", collectionName);
    return await service.Execute(req);
}