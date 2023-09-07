using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestCategory("BinarySearch")]
    [TestCategory("AStar")]
    [TestClass]
    public class Day15Test
    {
        readonly string input = Util.GetInput<Day15>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ElfBattle_Part1_Regression()
        {
            Assert.AreEqual(225096, Day15.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ElfBattle_Part2_Regression()
        {
            Assert.AreEqual(35354, Day15.Part2(input));
        }
    }
}
