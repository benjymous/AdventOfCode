using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day12Test
    {
        string input = Util.GetInput<Day12>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Rain_Part1_Regression()
        {
            Assert.AreEqual(2057, Day12.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Rain_Part2_Regression()
        {
            Assert.AreEqual(71504, Day12.Part2(input));
        }
    }
}
