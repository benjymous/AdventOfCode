using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day21Test
    {
        readonly string input = Util.GetInput<Day21>();

        [TestCategory("Regression")]
        [TestMethod]
        public void Enhancement_Part1_Regression()
        {
            Assert.AreEqual(173, Day21.Part1(input));
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void Enhancement_Part2_Regression()
        {
            Assert.AreEqual(2456178, Day21.Part2(input));
        }
    }
}
