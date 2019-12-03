using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestClass]
    public class Day05Test
    {
        [DataRow("aA", 0)]
        [DataRow("abBA", 0)]
        [DataRow("abAB", 4)]
        [DataRow("aabAAB", 6)]
        [DataRow("dabAcCaCBAcCcaDA", 10)]
        public void Polymer01Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day05.Part1(input));
        }

        [DataRow("dabAcCaCBAcCcaDA", 4)]
        [DataTestMethod]
        public void Polymer02Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day05.Part2(input));
        }

        [DataTestMethod]
        public void Polymer_Part1_Regression()
        {
            var d = new Day05();
            var input = Util.GetInput(d);
            Assert.AreEqual(9202, Day05.Part1(input));
        }

        [DataTestMethod]
        public void Polymer_Part2_Regression()
        {
            var d = new Day05();
            var input = Util.GetInput(d);
            Assert.AreEqual(6394, Day05.Part2(input));
        }

    }
}
