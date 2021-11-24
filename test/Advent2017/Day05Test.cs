using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day05Test
    {
        string input = Util.GetInput<Day05>();

        [TestCategory("Test")]
        [DataRow("0,3,0,1,-3", 5)]
        [DataTestMethod]
        public void Twisty01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2017.Day05.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("0,3,0,1,-3", 10)]
        [DataTestMethod]
        public void Twisty02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2017.Day05.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Twisty_Part1_Regression()
        {
            Assert.AreEqual(387096, Advent2017.Day05.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Twisty_Part2_Regression()
        {
            Assert.AreEqual(28040648, Advent2017.Day05.Part2(input));
        }

    }
}
