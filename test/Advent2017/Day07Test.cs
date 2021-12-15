using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day07Test
    {
        string input = Util.GetInput<Day07>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part1_Regression()
        {
            Assert.AreEqual("aapssr", Day07.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Part2_Regression()
        {
            Assert.AreEqual(1458, Day07.Part2(input));
        }
    }
}
