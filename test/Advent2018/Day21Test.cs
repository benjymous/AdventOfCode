using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day21Test
    {
        readonly string input = Util.GetInput<Day21>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void TimeUnderflow_Part1_Regression()
        {
            Assert.AreEqual(1797184, Day21.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void TimeUnderflow_Part2_Regression()
        {
            Assert.AreEqual(11011493, Day21.Part2(input));
        }
    }
}
