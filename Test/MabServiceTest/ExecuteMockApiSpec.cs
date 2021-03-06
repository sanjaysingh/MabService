﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Web.Http.Hosting;
using System.Web.Http;
using MabService.Data;
using MabService.Shared;
using MabService;
using System.Web.Http.Routing;
using MabService.Common;
using System.Collections.Generic;

namespace MabServiceTest
{
    [TestClass]
    public class ExecuteMockApiSpec
    {
        private InMemoryAzureTableMockApiRepository repo = new InMemoryAzureTableMockApiRepository();
        private ExecuteMockApiService executeMockApiService;
        private const string collectionName = "MyCollection";

        [TestInitialize]
        public void TestInitialize()
        {
            this.repo.CreateCollectionAsync(collectionName).Wait();
            var logger = new NullLogger();
            var languageBindingFactory = new LanguageBindingFactory(logger);
            this.executeMockApiService = new ExecuteMockApiService(logger, this.repo, languageBindingFactory);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task ExecuteMockApiWithValidJsonRequestWithNumericShouldBeOk()
        {
            string url = @"http://localhost/MyCollection/math/addition";
            var name = "AddTwoNumber";
            var routeTemplate = "math/addition";
            var verb = MockApiHttpVerb.Post;
            var body = @"function run(req, res) {
                            res.send( {result: req.content.num1 + req.content.num2});
                        }";
            await AddMockApi(new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript));

            var response = await RunMockApi(url, routeTemplate, new { num1 = 10, num2 = 50 });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            double responsePayload = (await response.Content.ReadAsAsync<dynamic>()).result;

            responsePayload.Should().Be(60);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task ExecuteMockApiWithValidJsonRequestWithStringsShouldBeOk()
        {
            string url = @"http://localhost/MyCollection/stringreverse";
            var name = "reverse";
            var routeTemplate = "stringreverse";
            var verb = MockApiHttpVerb.Post;
            var body = @"function run(req, res) {
                            res.send( {result: req.content.data.split('').reverse().join('')});
                        }";

            var mockApi = await AddMockApi(new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript));
            var response = await RunMockApi(url, routeTemplate, new { data = "adam" });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responsePayload = (await response.Content.ReadAsAsync<dynamic>());
            string result = responsePayload.result;
            result.Should().Be("mada");
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task ExecuteMockApiWithValidJsonRequestWithBooleanShouldBeOk()
        {
            string url = @"http://localhost/MyCollection/not";
            var name = "reverse";
            var routeTemplate = "not";
            var verb = MockApiHttpVerb.Post;
            var body = @"function run(req, res) {
                            res.send( {result: !req.content.data});
                        }";

            var mockApi = await AddMockApi(new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript));
            var response = await RunMockApi(url, routeTemplate, new { data = true });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responsePayload = (await response.Content.ReadAsAsync<dynamic>());
            bool result = responsePayload.result;
            result.Should().BeFalse();
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task ExecuteMockApiWithValidNestedJsonRequestShouldBeOk()
        {
            string url = @"http://localhost/MyCollection/not";
            var name = "reverse";
            var routeTemplate = "not";
            var verb = MockApiHttpVerb.Post;
            var body = @"function run(req, res) {
                            var response = req.content;
                            response.addition.result = response.addition.num1 + response.addition.num2;
                            response.substraction.result = response.substraction.num1 - response.substraction.num2;
                            res.send( response);
                        }";
            var content = new { addition = new { num1 = 2, num2 = 4 }, substraction = new { num1 = 10, num2 = 5 } };
            var mockApi = await AddMockApi(new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript));
            var httpResponse = await RunMockApi(url, routeTemplate, content);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            dynamic response = (await httpResponse.Content.ReadAsAsync<dynamic>());
            double additionResult = response.addition.result;
            double substractionResult = response.substraction.result;
            additionResult.Should().Be(6);
            substractionResult.Should().Be(5);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task ExecuteNonMatchingMockApiUrlShouldReturnNotFound()
        {
            string url = @"http://localhost/MyCollection/math/invalidurl";
            var name = "AddTwoNumber";
            var routeTemplate = "math/addition";
            var verb = MockApiHttpVerb.Post;
            var body = @"function run(req, res) {
                            res.send( {result: req.content.num1 + req.content.num2});
                        }";
            await AddMockApi(new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript));

            var response = await RunMockApi(url, routeTemplate, new { num1 = 10, num2 = 50 });
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task ExecuteNonMatchingMockApiHttpMethodShouldReturnMethodNotAllowed()
        {
            string url = @"http://localhost/MyCollection/math/sum";
            var name = "AddTwoNumber";
            var routeTemplate = "math/sum";
            var verb = MockApiHttpVerb.Post;
            var body = @"function run(req, res) {
                            res.send( {result: req.payload.num1 + req.payload.num2});
                        }";
            await AddMockApi(new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript));

            var response = await RunMockApi(url, routeTemplate, new { num1 = 10, num2 = 50 }, httpMethod: HttpMethod.Get);
            response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task ExecuteMockApiWithOneRouteTemplateDataShouldWorkAsExpected()
        {
            string url = @"http://localhost/MyCollection/math/square/5";
            var name = "AddTwoNumber";
            var routeTemplate = "math/square/{num}";
            var verb = MockApiHttpVerb.Post;
            var body = @"function run(req, res) {
                            res.send( {result: req.param.num * req.param.num});
                        }";
            await AddMockApi(new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript));

            var httpResponse = await RunMockApi(url, routeTemplate, httpMethod: HttpMethod.Post);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            dynamic response = (await httpResponse.Content.ReadAsAsync<dynamic>());
            double result = response.result;
            result.Should().Be(25);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task ExecuteMockApiWithTwoRouteTemplateDataShouldWorkAsExpected()
        {
            string url = @"http://localhost/MyCollection/math/reverse/5/6";
            var name = "AddTwoNumber";
            var routeTemplate = "math/reverse/{num1}/{num2}";
            var verb = MockApiHttpVerb.Get;
            var body = @"function run(req, res) {
                            res.send( {num1: parseInt(req.param.num2), num2: parseInt(req.param.num1)});
                        }";
            await AddMockApi(new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript));

            var httpResponse = await RunMockApi(url, routeTemplate, httpMethod: HttpMethod.Get);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            dynamic response = (await httpResponse.Content.ReadAsAsync<dynamic>());
            
            ((double)response.num1).Should().Be(6);
            ((double)response.num2).Should().Be(5);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task ExecuteMockApiWithOneQueryStringTemplateDataShouldWorkAsExpected()
        {
            string url = @"http://localhost/MyCollection/math/reverse?data=abcd";
            var name = "AddTwoNumber";
            var routeTemplate = "math/reverse"; 
            var verb = MockApiHttpVerb.Get;
            var body = @"function run(req, res) {
                            res.send( {reverse:req.query.data.split('').reverse().join('') });
                        }";
            await AddMockApi(new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript));

            var httpResponse = await RunMockApi(url, routeTemplate, httpMethod: HttpMethod.Get);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            dynamic response = (await httpResponse.Content.ReadAsAsync<dynamic>());

            ((string)response.reverse).Should().Be("dcba");
            
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task ExecuteNonCompilingMockApiShouldReturnBadRequest()
        {
            string url = @"http://localhost/MyCollection/math/addition";
            var name = "AddTwoNumber";
            var routeTemplate = "math/addition";
            var verb = MockApiHttpVerb.Post;
            var body = @"function run(req, res) {
                            error
                            res.send( {result: req.payload.num1 + req.payload.num2});
                        }}";
            await AddMockApi(new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript));

            var response = await RunMockApi(url, routeTemplate, new { num1 = 10, num2 = 50 });
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCategory("Acceptance Test")]
        [TestMethod]
        public async Task ExecuteMockApiWithUnhandledExceptionShouldReturnInternalServerError()
        {
            string url = @"http://localhost/MyCollection/math/addition";
            var name = "AddTwoNumber";
            var routeTemplate = "math/addition";
            var verb = MockApiHttpVerb.Post;
            var body = @"function run(req, res) {
                            res.send( {result: req.payload.num1 + req.payload.num2});
                        }";
            await AddMockApi(new MockApiModel(name, routeTemplate, body, verb, MockApiLanguage.JavaScript));

            var response = await RunMockApi(url, routeTemplate, new { badData = 5 });
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        private async Task<MockApiModel> AddMockApi(MockApiModel mockApi)
        {
            return await this.repo.AddMockApiAsync(mockApi, collectionName);
        }

        private async Task<HttpResponseMessage> RunMockApi(string url,
                                                            string routeTemplate,
                                                            object requestBody = null,
                                                            string collectionName = collectionName,
                                                            HttpMethod httpMethod = null)
        {
            var routeValues = new HttpRouteValueDictionary() { { "collectionName", collectionName } };
            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod ?? HttpMethod.Post, url);
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            if (requestBody != null)
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            }
            requestMessage.SetRouteData(new HttpRouteData(new HttpRoute(routeTemplate), routeValues));

            return await this.executeMockApiService.Execute(requestMessage);
        }
    }
}
