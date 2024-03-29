using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day02Test
    {
        readonly string input = Util.GetInput<Day02>();

        [TestCategory("Test")]
        [DataRow("forward 5\ndown 5\nforward 8\nup 3\ndown 8\nforward 2", 150)]
        [DataTestMethod]
        public void Dive01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day02.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("forward 5\ndown 5\nforward 8\nup 3\ndown 8\nforward 2\n", 900)]
        [DataTestMethod]
        public void Dive02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day02.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Dive_Part1_Regression()
        {
            Assert.AreEqual(1383564, Day02.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Dive_Part2_Regression()
        {
            Assert.AreEqual(1488311643, Day02.Part2(input));
        }
    }
}
