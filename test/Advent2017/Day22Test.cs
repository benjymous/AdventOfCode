using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day22Test
    {
        readonly string input = Util.GetInput<Day22>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Infection_Part1_Regression()
        {
            Assert.AreEqual(5404, Day22.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Infection_Part2_Regression()
        {
            Assert.AreEqual(2511672, Day22.Part2(input));
        }
    }
}
