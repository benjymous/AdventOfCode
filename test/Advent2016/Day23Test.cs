using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestCategory("BunnyCPU")]
    [TestClass]
    public class Day23Test
    {
        readonly string input = Util.GetInput<Day23>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void SafeCrack_Part1_Regression()
        {
            Assert.AreEqual(11004, Day23.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void SafeCrack_Part2_Regression()
        {
            Assert.AreEqual(479007564, Day23.Part2(input));
        }
    }
}
