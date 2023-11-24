using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestCategory("PackedVect")]
    [TestClass]
    public class Day14Test
    {
        readonly string input = Util.GetInput<Day14>();
        readonly string test = "498,4 -> 498,6 -> 496,6\n503,4 -> 502,4 -> 502,9 -> 494,9";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Sand01Test()
        {
            Assert.AreEqual(24, Day14.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Sand02Test()
        {
            Assert.AreEqual(93, Day14.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Sand_Part1_Regression()
        {
            Assert.AreEqual(683, Day14.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Sand_Part2_Regression()
        {
            Assert.AreEqual(28821, Day14.Part2(input));
        }
    }
}
