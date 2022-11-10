using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day20Test
    {
        readonly string input = Util.GetInput<Day20>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Deliveries_Part1_Regression()
        {
            Assert.AreEqual(831600, Day20.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Deliveries_Part2_Regression()
        {
            Assert.AreEqual(884520, Day20.Part2(input));
        }
    }
}
