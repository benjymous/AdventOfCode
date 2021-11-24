using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day13Test
    {
        string input = Util.GetInput<Day13>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Shuttle_Part1_Regression()
        {
            Assert.AreEqual(171UL, Day13.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Shuttle_Part2_Regression()
        {
            Assert.AreEqual(539746751134958UL, Day13.Part2(input));
        }
    }
}
