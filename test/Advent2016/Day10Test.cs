using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day10Test
    {
        readonly string input = Util.GetInput<Day10>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Microbots_Part1_Regression()
        {
            Assert.AreEqual("bot 27", Day10.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Microbots_Part2_Regression()
        {
            Assert.AreEqual(13727, Day10.Part2(input));
        }
    }
}
