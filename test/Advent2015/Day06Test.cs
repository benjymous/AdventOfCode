using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day06Test
    {
        readonly string input = Util.GetInput<Day06>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void MillionLights_Part1_Regression()
        {
            Assert.AreEqual(569999, Day06.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void MillionLights_Part2_Regression()
        {
            Assert.AreEqual(17836115, Day06.Part2(input));
        }
    }
}
