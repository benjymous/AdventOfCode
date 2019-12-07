using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVII.Test
{
    [TestClass]
    public class Day05Test
    {
        [DataRow("0,3,0,1,-3", 5)]
        [DataTestMethod]
        public void Twisty01Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVII.Day05.Part1(input));
        }

        [DataRow("0,3,0,1,-3", 10)]
        [DataTestMethod]
        public void Twisty02Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVII.Day05.Part2(input));
        }

        [DataTestMethod]
        public void Twisty_Part1_Regression()
        {
            var d = new MMXVII.Day05();
            var input = Util.GetInput(d);
            Assert.AreEqual(387096, MMXVII.Day05.Part1(input));
        }

        [DataTestMethod]
        public void Twisty_Part2_Regression()
        {
            var d = new MMXVII.Day05();
            var input = Util.GetInput(d);
            Assert.AreEqual(28040648, MMXVII.Day05.Part2(input));
        }

    }
}
