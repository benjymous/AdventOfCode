using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestCategory("RegexFactory")]
    [TestClass]
    public class Day13Test
    {
        readonly string input = Util.GetInput<Day13>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void TablePlan_Part1_Regression()
        {
            Assert.AreEqual(618, Day13.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void TablePlan_Part2_Regression()
        {
            Assert.AreEqual(601, Day13.Part2(input));
        }
    }
}
