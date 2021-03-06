using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day09Test
    {
        string input = Util.GetInput<Day09>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part1_Regression()
        {
            Assert.AreEqual(107035, Day09.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part2_Regression()
        {
            Assert.AreEqual(11451628995, Day09.Part2(input));
        }
    }
}
