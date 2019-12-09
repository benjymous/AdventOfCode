using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVII.Test
{
    [TestClass]
    public class Day04Test
    {
        string input = Util.GetInput<Day04>();

        [TestCategory("Test")]
        [DataRow("aa bb cc dd ee", true)]
        [DataRow("aa bb cc dd aa", false)]
        [DataRow("aa bb cc dd aaa", true)]
        [DataTestMethod]
        public void Entropy01Test(string input, bool expected)
        {
            Assert.AreEqual(expected, MMXVII.Day04.ValidationRule2(input));
        }

        [TestCategory("Test")]
        [DataRow("abcde fghij", true)]
        [DataRow("abcde xyz ecdab", false)]
        [DataRow("a ab abc abd abf abj", true)]
        [DataRow("iiii oiii ooii oooi oooo", true)]
        [DataRow("oiii ioii iioi iiio", false)]
        [DataTestMethod]
        public void Entropy02Test(string input, bool expected)
        {
            Assert.AreEqual(expected, MMXVII.Day04.ValidationRule2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Entropy_Part1_Regression()
        {
            Assert.AreEqual(325, MMXVII.Day04.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Entropy_Part2_Regression()
        {
            Assert.AreEqual(119, MMXVII.Day04.Part2(input));
        }

    }
}
