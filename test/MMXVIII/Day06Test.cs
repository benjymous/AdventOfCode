using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestClass]
    public class Day06Test
    {
        // [DataRow("1, 1\n1, 6\n8, 3\n3, 4\n5, 5\n8, 9", 17)]
        // [DataTestMethod]
        // public void Coordinates01Test(string input, int expected)
        // {
        //     Assert.AreEqual(expected, MMXVIII.Day06.Part1(input));
        // }

        [DataRow("1, 1\n1, 6\n8, 3\n3, 4\n5, 5\n8, 9", 32, 16)]
        [DataTestMethod]
        public void Coordinates02Test(string input, int maxValue, int expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day06.Part2(input, maxValue));
        }

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
