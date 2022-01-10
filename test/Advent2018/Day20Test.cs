using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day20Test
    {
        string input = Util.GetInput<Day20>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part1_Regression()
        {
            Assert.AreEqual(3545, Day20.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part2_Regression()
        {
            Assert.AreEqual(7838, Day20.Part2(input));
        }
    }
}
