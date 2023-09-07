using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day17Test
    {
        readonly string input = Util.GetInput<Day17>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Noggify_Part1_Regression()
        {
            Assert.AreEqual(654, Day17.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Noggify_Part2_Regression()
        {
            Assert.AreEqual(57, Day17.Part2(input));
        }
    }
}
