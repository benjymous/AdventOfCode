using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day02Test
    {
        readonly string input = Util.GetInput<Day02>();

        readonly string test = "A Y\nB X\nC Z";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Scissors01Test()
        {
            Assert.AreEqual(15, Day02.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Scissors02Test()
        {
            Assert.AreEqual(12, Day02.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Scissors_Part1_Regression()
        {
            Assert.AreEqual(13682, Day02.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Scissors_Part2_Regression()
        {
            Assert.AreEqual(12881, Day02.Part2(input));
        }
    }
}
