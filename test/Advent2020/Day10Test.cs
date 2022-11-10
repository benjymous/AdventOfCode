using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day10Test
    {
        readonly string input = Util.GetInput<Day10>();

        [TestCategory("Test")]
        [DataRow("16,10,15,5,1,11,7,19,6,12,4", 35)]
        [DataRow("28,33,18,42,31,14,46,20,48,47,24,23,49,45,19,38,39,11,1,32,25,35,8,17,7,9,4,2,34,10,3", 220)]
        [DataTestMethod]
        public void Adapters01Test(string input, Int64 expected)
        {
            Assert.AreEqual(expected, Advent2020.Day10.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("16,10,15,5,1,11,7,19,6,12,4", 8)]
        [DataRow("28,33,18,42,31,14,46,20,48,47,24,23,49,45,19,38,39,11,1,32,25,35,8,17,7,9,4,2,34,10,3", 19208)]
        [DataTestMethod]
        public void Adapters02Test(string input, Int64 expected)
        {
            Assert.AreEqual(expected, Advent2020.Day10.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Adapters_Part1_Regression()
        {
            Assert.AreEqual(2812, Day10.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Adapters_Part2_Regression()
        {
            Assert.AreEqual(386869246296064, Day10.Part2(input));
        }
    }
}
