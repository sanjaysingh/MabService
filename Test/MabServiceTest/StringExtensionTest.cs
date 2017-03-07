using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using MabService.Shared;

namespace MabServiceTest
{
    [TestClass]
    public class StringExtensionTest
    {
        [TestMethod]
        public void IsNullOrWhiteSpace_ForNull_ShouldBeTrue()
        {
            ((string)null).IsNullOrWhiteSpace().Should().BeTrue();
        }

        [TestMethod]
        public void IsNullOrWhiteSpace_ForNonEmpty_ShouldBeFalse()
        {
            "A valid string".IsNullOrWhiteSpace().Should().BeFalse();
        }

        [TestMethod]
        public void IsAlphanumeric_ForAlphanumeric_ShouldBeTrue()
        {
            "ABC123".IsAlphanumeric().Should().BeTrue();
        }
        [TestMethod]
        public void IsAlphanumeric_ForAlphabetsOnly_ShouldBeTrue()
        {
            "ABC".IsAlphanumeric().Should().BeTrue();
        }
        [TestMethod]
        public void IsAlphanumeric_ForSpecialCharacters_ShouldBeFalse()
        {
            "ABC$".IsAlphanumeric().Should().BeFalse();
        }
        [TestMethod]
        public void IsInLength_Between2And5_ShouldBeTrueFor4()
        {
            "ABC$".IsInLength(2, 5).Should().BeTrue();
        }
        [TestMethod]
        public void IsInLength_Between2And5_ShouldBeTrueFor2()
        {
            "AB".IsInLength(2, 5).Should().BeTrue();
        }
        [TestMethod]
        public void IsInLength_Between2And5_ShouldBeTrueFor5()
        {
            "ABCDE".IsInLength(2, 5).Should().BeTrue();
        }
        [TestMethod]
        public void IsInLength_Between2And5_ShouldBeFalseFor1()
        {
            "A".IsInLength(2, 5).Should().BeFalse();
        }
        [TestMethod]
        public void IsInLength_Between2And5_ShouldBeFalseFor6()
        {
            "ABCDEF".IsInLength(2, 5).Should().BeFalse();
        }
    }
}
