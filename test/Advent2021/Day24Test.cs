using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day24Test
    {
        string input = Util.GetInput<Day24>();

        [TestCategory("Test")]
        [DataTestMethod]
        [DataRow(49917929934999)]
        [DataRow(11911316711816)]

        public void MonadCalcTest(long input)
        {
            Assert.IsTrue(Day24.ValidateMonad(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ALU_Part1_Regression()
        {
            Assert.AreEqual(49917929934999, Day24.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ALU_Part2_Regression()
        {
            Assert.AreEqual(11911316711816, Day24.Part2(input));
        }
    }
}
