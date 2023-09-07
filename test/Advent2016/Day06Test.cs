using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day06Test
    {
        readonly string input = Util.GetInput<Day06>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Signals_Part1_Regression()
        {
            Assert.AreEqual("zcreqgiv", Day06.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Signals_Part2_Regression()
        {
            Assert.AreEqual("pljvorrk", Day06.Part2(input));
        }
    }
}
