using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day07Test
    {
        readonly string input = Util.GetInput<Day07>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Circuit_Part1_Regression()
        {
            Assert.AreEqual(16076, Day07.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Circuit_Part2_Regression()
        {
            Assert.AreEqual(2797, Day07.Part2(input));
        }
    }
}
