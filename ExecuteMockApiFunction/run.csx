﻿#load "..\common.csx"

using System.Net;
using MabService;
using MabService.Shared;
using MabService.FunctionsHelper;
using Jint;
using Jint.Native;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"ExecuteMockApiService is processing a request. RequestUri={req.RequestUri}");
    var service = ServiceFactory.CreateExecuteMockApiService(log);
    return await service.Execute(req);
}