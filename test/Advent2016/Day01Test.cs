using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day01Test
    {
        readonly string input = Util.GetInput<Day01>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void BunnyPath_Part1_Regression()
        {
            Assert.AreEqual(273, Day01.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void BunnyPath_Part2_Regression()
        {
            Assert.AreEqual(115, Day01.Part2(input));
        }
    }
}
