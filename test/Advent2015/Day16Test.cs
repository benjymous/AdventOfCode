using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day16Test
    {
        readonly string input = Util.GetInput<Day16>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Sues_Part1_Regression()
        {
            Assert.AreEqual(103, Day16.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Sues_Part2_Regression()
        {
            Assert.AreEqual(405, Day16.Part2(input));
        }
    }
}
