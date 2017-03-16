using MabService.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
namespace MabService.Common
{
    /// <summary>
    /// Route utility class
    /// </summary>
    public static class RouteUtil
    {
        private const char routeSeperator = '/';

        /// <summary>
        /// Gets the segments.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>route segments</returns>
        public static IEnumerable<string> GetSegments(string route)
        {
            return route.Split(new char[] { routeSeperator }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Builds the route.
        /// </summary>
        /// <param name="segments">The segments.</param>
        /// <returns></returns>
        public static string BuildRoute(IEnumerable<string> segments)
        {
            return string.Join(routeSeperator.ToString(), segments);
        }

        /// <summary>
        /// Determines whether [is valid route template] [the specified route template].
        /// </summary>
        /// <param name="routeTemplate">The route template.</param>
        /// <returns>
        ///   <c>true</c> if [is valid route template] [the specified route template]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInvalidRouteTemplate(string routeTemplate)
        {
            if (routeTemplate.IsNullOrWhiteSpace() || routeTemplate.Length > Constants.MaxApiTemplateLength) return true;

            return GetSegments(routeTemplate).Any(currSegment => IsInvalidRouteSegment(currSegment));
        }

        /// <summary>
        /// Matches the template.
        /// </summary>
        /// <param name="actualRoute">The actual route.</param>
        /// <param name="routeTemplate">The route template.</param>
        /// <returns>returns key value dictionary for template keys and their matching value</returns>
        public static IDictionary<string, string> MatchTemplate(string actualRoute, string routeTemplate)
        {
            var actualRouteSegments = GetSegments(actualRoute).ToArray();
            var routeTemplateSegments = GetSegments(routeTemplate).ToArray();

            if (actualRouteSegments.Length != routeTemplateSegments.Length) return null;
            var routeKeyValueMap = new Dictionary<string, string>();
            for(var i=0; i< actualRouteSegments.Length; i++)
            {
                var currActualRouteSegment = actualRouteSegments[i];
                var currTemplateRouteSegment = routeTemplateSegments[i];
                if (!IsTemplateSegment(currTemplateRouteSegment))
                {
                    if(!currActualRouteSegment.Equals(currTemplateRouteSegment, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                routeKeyValueMap[GetRouteKeyForTemplateSegment(currTemplateRouteSegment)] = currActualRouteSegment;
            }

            return routeKeyValueMap;
        }

        /// <summary>
        /// Gets the route key for template segment.
        /// </summary>
        /// <param name="templateSegment">The template segment.</param>
        /// <returns>the key for the template segment</returns>
        private static string GetRouteKeyForTemplateSegment(string templateSegment)
        {
            return templateSegment.Substring(1, templateSegment.Length - 2);
        }

        /// <summary>
        /// Determines whether [is template segment] [the specified route template segment].
        /// </summary>
        /// <param name="routeTemplateSegment">The route template segment.</param>
        /// <returns>
        ///   <c>true</c> if [is template segment] [the specified route template segment]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsTemplateSegment(string routeTemplateSegment)
        {
            return routeTemplateSegment.StartsWith("{") && routeTemplateSegment.EndsWith("}");
        }

        /// <summary>
        /// Determines whether [is invalid route segment] [the specified route segment].
        /// </summary>
        /// <param name="routeSegment">The route segment.</param>
        /// <returns>
        ///   <c>true</c> if [is invalid route segment] [the specified route segment]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsInvalidRouteSegment(string routeSegment)
        {
            var segmentName = IsTemplateSegment(routeSegment) ? GetRouteKeyForTemplateSegment(routeSegment) : routeSegment;
            return segmentName.IsNotAlphanumeric();
        }
    } 
}
