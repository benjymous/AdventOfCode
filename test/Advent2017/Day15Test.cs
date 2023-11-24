using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day15Test
    {
        readonly string input = Util.GetInput<Day15>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Duel_Part1_Regression()
        {
            Assert.AreEqual(577, Day15.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Duel_Part2_Regression()
        {
            Assert.AreEqual(316, Day15.Part2(input));
        }
    }
}
