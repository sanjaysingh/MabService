#r "MabService.dll"
#r "MabService.Domain.dll"

using System.Net;
using MabService;
using MabService.Domain;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");
    var service = ServiceFactory.CreateMockApiDefinitionService(log);

    await service.CreateCollectionAsync(req);
}