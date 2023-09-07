using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day14Test
    {
        readonly string input = Util.GetInput<Day14>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void DeerRace_Part1_Regression()
        {
            Assert.AreEqual(2696, Day14.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void DeerRace_Part2_Regression()
        {
            Assert.AreEqual(1084, Day14.Part2(input));
        }
    }
}
