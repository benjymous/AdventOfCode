using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day23Test
    {
        readonly string input = Util.GetInput<Day23>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void TuringLock_Part1_Regression()
        {
            Assert.AreEqual(184, Day23.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void TuringLock_Part2_Regression()
        {
            Assert.AreEqual(231, Day23.Part2(input));
        }
    }
}
