using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVII.Test
{
    [TestClass]
    public class Day05Test
    {
        string input = Util.GetInput<Day05>();

        [TestCategory("Test")]
        [DataRow("0,3,0,1,-3", 5)]
        [DataTestMethod]
        public void Twisty01Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVII.Day05.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("0,3,0,1,-3", 10)]
        [DataTestMethod]
        public void Twisty02Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVII.Day05.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Twisty_Part1_Regression()
        {
            Assert.AreEqual(387096, MMXVII.Day05.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Twisty_Part2_Regression()
        {
            Assert.AreEqual(28040648, MMXVII.Day05.Part2(input));
        }

    }
}
