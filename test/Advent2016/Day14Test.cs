using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day14Test
    {
        string input = Util.GetInput<Day14>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part1_Regression()
        {
            Assert.AreEqual(15168, Day14.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part2_Regression()
        {
            Assert.AreEqual(20864, Day14.Part2(input));
        }
    }
}
