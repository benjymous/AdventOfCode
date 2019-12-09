using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day06Test
    {
        string input = Util.GetInput<Day06>();

        // [DataRow("1, 1\n1, 6\n8, 3\n3, 4\n5, 5\n8, 9", 17)]
        // [DataTestMethod]
        // public void Coordinates01Test(string input, int expected)
        // {
        //     Assert.AreEqual(expected, MMXVIII.Day06.Part1(input));
        // }

        [TestCategory("Test")]
        [DataRow("1, 1\n1, 6\n8, 3\n3, 4\n5, 5\n8, 9", 32, 16)]
        [DataTestMethod]
        public void Coordinates02Test(string input, int maxValue, int expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day06.Part2(input, maxValue));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Chronal_Part1_Regression()
        {
            Assert.AreEqual(3890, Day06.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Chronal_Part2_Regression()
        {
            Assert.AreEqual(40284, Day06.Part2(input));
        }

    }
}
