using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day15Test
    {
        readonly string input = Util.GetInput<Day15>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part1_Regression()
        {
            Assert.AreEqual(13882464, Day15.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part2_Regression()
        {
            Assert.AreEqual(11171160, Day15.Part2(input));
        }
    }
}
