#load "..\common.csx"

using System.Net;
using MabService;
using MabService.Shared;
using MabService.FunctionsHelper;
using System.Web.Http.Routing;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string collectionName, TraceWriter log)
{
    log.Info($"GetCollectionReferenceService is processing a request. RequestUri={req.RequestUri}");
    var service = ServiceFactory.CreateGetCollectionReferenceService(log);
    req.GetRouteData().Values.Add("collectionName", collectionName);
    return await service.Execute(req);
}