using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestCategory("Solver")]
    [TestClass]
    public class Day11Test
    {
        readonly string input = Util.GetInput<Day11>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ChipFloors_Part1_Regression()
        {
            Assert.AreEqual(47, Day11.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ChipFloors_Part2_Regression()
        {
            Assert.AreEqual(71, Day11.Part2(input));
        }
    }
}
