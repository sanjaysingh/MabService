using MabService.Shared;
using System.Threading.Tasks;
using System.Net.Http;
using System.Reflection;
using MabService.Shared.Exceptions;
using System;
using System.Net;
using System.Linq;
using System.Web.Http.Controllers;

namespace MabService.Common
{
    /// <summary>
    /// Base class for all servies
    /// </summary>
    public abstract class MockApiServiceBase : IMockApiService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockApiServiceBase"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected MockApiServiceBase(ILogger logger, IMockApiRepository mockApiRepo)
        {
            this.Logger = logger;
            this.MockApiRepo = mockApiRepo;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets the mock API repo.
        /// </summary>
        /// <value>
        /// The mock API repo.
        /// </value>
        protected IMockApiRepository MockApiRepo { get; }

        /// <summary>
        /// Executes the specified req.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns>http response message</returns>
        public async Task<HttpResponseMessage> Execute(HttpRequestMessage req)
        {
            HttpResponseMessage response;
            try
            {
                this.ValidateRequest(req);
                response = await this.ExecuteInternal(req);
            }
            catch (ResourceNotFoundException ex)
            {
                this.Logger.Error(ex.Message, ex);
                response = req.CreateResponse(HttpStatusCode.NotFound, ErrorResponseResource.From(ex));
            }
            catch (ValidationException ex)
            {
                this.Logger.Error(ex.Message, ex);
                response = req.CreateResponse(HttpStatusCode.BadRequest, ErrorResponseResource.From(ex));
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.Message, ex);
                response = req.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponseResource.From(Constants.InternalServerErrorId, Constants.InternalServerErrorMessage));
            }

            return response;
        }

        /// <summary>
        /// Overrides the actual implementation of the service
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        protected abstract Task<HttpResponseMessage> ExecuteInternal(HttpRequestMessage req);

        /// <summary>
        /// Gets the route value.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <param name="routeKey">The route key.</param>
        /// <returns>the value for the given route key</returns>
        protected static string GetRouteValue(HttpRequestMessage req, string routeKey)
        {
            var routeData = req.GetRouteData();
            if (routeData.Values != null && routeData.Values.ContainsKey(routeKey))
            {
                return routeData.Values[routeKey].ToString();
            }

            return string.Empty;
        }

        private void ValidateRequest(HttpRequestMessage req)
        {
            var handlerMethod = this.GetType().GetMethod("ExecuteInternal", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            var isAllowedHandler = handlerMethod.GetCustomAttributes(true).Any(attr =>
            {
                var actionMethodProvider = attr as IActionHttpMethodProvider;
                if (actionMethodProvider != null)
                {
                    return actionMethodProvider.HttpMethods.Contains(req.Method);
                }
                return false;
            });

            if (!isAllowedHandler)
            {
                throw new ResourceNotFoundException();
            }
        }
    }
}
