using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using MabService.Shared;
using MabService.Common;

namespace MabServiceTest
{
    [TestClass]
    public class RouteUtilTest
    {
        [TestMethod]
        [TestCategory("Class Test")]
        public void GetSegments_For3SegmentsRoute_ShouldReturn3Segments()
        {
            RouteUtil.GetSegments("math/addition/2/3").ShouldAllBeEquivalentTo(new string[] {"math","addition", "2", "3" });
        }

        [TestMethod]
        [TestCategory("Class Test")]
        public void GetSegments_For1SegmentsRoute_ShouldReturn1Segment()
        {
            RouteUtil.GetSegments("math").ShouldAllBeEquivalentTo(new string[] { "math" });
        }

        [TestMethod]
        [TestCategory("Class Test")]
        public void MatchTemplate_WithMatching2TempateSegment_ShouldReturn2KeyValuePairs()
        {
            var routeMatch = RouteUtil.MatchTemplate("math/addition/2/3", "math/addition/{num1}/{num2}");
            routeMatch["num1"].Should().Be("2");
            routeMatch["num2"].Should().Be("3");
        }

        [TestMethod]
        [TestCategory("Class Test")]
        public void MatchTemplate_WithNonMatching2TempateSegment_ShouldReturnNull()
        {
            var routeMatch = RouteUtil.MatchTemplate("math/substraction/2/3", "math/addition/{num1}/{num2}");
            routeMatch.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("Class Test")]
        public void IsInvalidRouteTemplate_ValidRouteWithNoTemplateSegment_ShouldReturnFalse()
        {
            RouteUtil.IsInvalidRouteTemplate("math/addition").Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("Class Test")]
        public void IsInvalidRouteTemplate_ValidRouteWith2TemplateSegment_ShouldReturnFalse()
        {
            RouteUtil.IsInvalidRouteTemplate("math/addition/{num1}/{num2}").Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("Class Test")]
        public void IsInvalidRouteTemplate_InvalidChars_ShouldReturnTrue()
        {
            RouteUtil.IsInvalidRouteTemplate("math/add#ition/{num1}/{num2}").Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("Class Test")]
        public void IsInvalidRouteTemplate_UnbalancedBraces_ShouldReturnTrue()
        {
            RouteUtil.IsInvalidRouteTemplate("math/addition/{nu{m1}/{num2}").Should().BeTrue();
        }
    }
}
