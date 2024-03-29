using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day02Test
    {
        readonly string input = Util.GetInput<Day02>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Wrapping_Part1_Regression()
        {
            Assert.AreEqual(1586300, Day02.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Wrapping_Part2_Regression()
        {
            Assert.AreEqual(3737498, Day02.Part2(input));
        }
    }
}
