using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day15Test
    {
        readonly string input = Util.GetInput<Day15>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Capsules_Part1_Regression()
        {
            Assert.AreEqual(203660, Day15.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Capsules_Part2_Regression()
        {
            Assert.AreEqual(2408135, Day15.Part2(input));
        }
    }
}
