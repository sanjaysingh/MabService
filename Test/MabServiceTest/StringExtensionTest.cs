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
        public void IsNotAlphanumeric_ForAlphanumeric_ShouldBeFalse()
        {
            "ABC123".IsNotAlphanumeric().Should().BeFalse();
        }
        [TestMethod]
        public void IsNotAlphanumeric_ForAlphabetsOnly_ShouldBeFalse()
        {
            "ABC".IsNotAlphanumeric().Should().BeFalse();
        }
        [TestMethod]
        public void IsNotAlphanumeric_ForSpecialCharacters_ShouldBeTrue()
        {
            "ABC$".IsNotAlphanumeric().Should().BeTrue();
        }
        [TestMethod]
        public void IsNotInLength_Between2And5_ShouldBeFalseFor4()
        {
            "ABC$".IsNotInLength(2, 5).Should().BeFalse();
        }
        [TestMethod]
        public void IsNotInLength_Between2And5_ShouldBeFalseFor2()
        {
            "AB".IsNotInLength(2, 5).Should().BeFalse();
        }
        [TestMethod]
        public void IsNotInLength_Between2And5_ShouldBeFalseFor5()
        {
            "ABCDE".IsNotInLength(2, 5).Should().BeFalse();
        }
        [TestMethod]
        public void IsNotInLength_Between2And5_ShouldBeTrueFor1()
        {
            "A".IsNotInLength(2, 5).Should().BeTrue();
        }
        [TestMethod]
        public void IsNotInLength_Between2And5_ShouldBeTrueFor6()
        {
            "ABCDEF".IsNotInLength(2, 5).Should().BeTrue();
        }
    }
}
