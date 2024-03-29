using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day01Test
    {
        readonly string input = Util.GetInput<Day01>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Brackets_Part1_Regression()
        {
            Assert.AreEqual(138, Day01.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Brackets_Part2_Regression()
        {
            Assert.AreEqual(1771, Day01.Part2(input));
        }
    }
}
