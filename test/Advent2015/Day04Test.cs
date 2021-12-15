using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day04Test
    {
        string input = Util.GetInput<Day04>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part1_Regression()
        {
            Assert.AreEqual(254575, Day04.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part2_Regression()
        {
            Assert.AreEqual(1038736, Day04.Part2(input));
        }
    }
}
