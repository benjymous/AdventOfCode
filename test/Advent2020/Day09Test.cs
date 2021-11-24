using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day09Test
    {
        string input = Util.GetInput<Day09>();

        [TestCategory("Test")]
        [DataRow("35,20,15,25,47,40,62,55,65,95,102,117,150,182,127,219,299,277,309,576", 5, 127)]
        [DataTestMethod]
        public void Encoding01Test(string input, int preamble, int expected)
        {
            Assert.IsTrue((int)Advent2020.Day09.Part1(input, preamble) == expected);
        }

        [TestCategory("Test")]
        [DataRow("35,20,15,25,47,40,62,55,65,95,102,117,150,182,127,219,299,277,309,576", 5, 62)]
        [DataTestMethod]
        public void Encoding02Test(string input, int preamble, int expected)
        {
            Assert.IsTrue((int)Advent2020.Day09.Part2(input, preamble) == expected);
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Encoding_Part1_Regression()
        {
            Assert.AreEqual((Int64)1124361034, Day09.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Encoding_Part2_Regression()
        {
            Assert.AreEqual((Int64)129444555, Day09.Part2(input));
        }
    }
}
