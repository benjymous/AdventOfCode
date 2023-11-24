using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestCategory("Solver")]
    [TestClass]
    public class Day22Test
    {
        readonly string input = Util.GetInput<Day22>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Wizardry_Part1_Regression()
        {
            Assert.AreEqual(900, Day22.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Wizardry_Part2_Regression()
        {
            Assert.AreEqual(1216, Day22.Part2(input));
        }
    }
}
