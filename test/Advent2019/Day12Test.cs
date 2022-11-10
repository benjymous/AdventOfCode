using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestClass]
    public class Day12Test
    {
        readonly string input = Util.GetInput<Day12>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Painter_Part1_Regression()
        {
            Assert.AreEqual(8625, Day12.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Painter_Part2_Regression()
        {
            Assert.AreEqual(332477126821644, Day12.Part2(input));
        }

    }
}
