#load "..\common.csx"

using System.Net;
using MabService;
using MabService.Shared;
using MabService.FunctionsHelper;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"GetCollectionService is processing a request. RequestUri={req.RequestUri}");
    var service = ServiceFactory.CreateGetCollectionService(log);
    return await service.Execute(req);
}