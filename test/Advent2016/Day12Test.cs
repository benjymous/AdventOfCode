using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day12Test
    {
        string input = Util.GetInput<Day12>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part1_Regression()
        {
            Assert.AreEqual(318020, Day12.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part2_Regression()
        {
            Assert.AreEqual(9227674, Day12.Part2(input));
        }
    }
}
