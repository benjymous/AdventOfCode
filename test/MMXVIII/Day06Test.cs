using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestClass]
    public class Day06Test
    {
        // [DataRow("aA", 0)]
        // [DataRow("abBA", 0)]
        // [DataRow("abAB", 4)]
        // [DataRow("aabAAB", 6)]
        // [DataRow("dabAcCaCBAcCcaDA", 10)]
        // public void Polymer01Test(string input, int expected)
        // {
        //     Assert.AreEqual(expected, MMXVIII.Day05.Part1(input));
        // }

        // [DataRow("dabAcCaCBAcCcaDA", 4)]
        // [DataTestMethod]
        // public void Polymer02Test(string input, int expected)
        // {
        //     Assert.AreEqual(expected, MMXVIII.Day05.Part2(input));
        // }

        [DataTestMethod]
        public void Chronal_Part1_Regression()
        {
            var d = new Day06();
            var input = Util.GetInput(d);
            Assert.AreEqual(3890, Day06.Part1(input));
        }

        [DataTestMethod]
        public void Chronal_Part2_Regression()
        {
            var d = new Day06();
            var input = Util.GetInput(d);
            Assert.AreEqual(40284, Day06.Part2(input));
        }

    }
}
