using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day17Test
    {
        readonly string input = Util.GetInput<Day17>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Resevoir_Part1_Regression()
        {
            Assert.AreEqual(42429, Day17.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Resevoir_Part2_Regression()
        {
            Assert.AreEqual(35998, Day17.Part2(input));
        }
    }
}
