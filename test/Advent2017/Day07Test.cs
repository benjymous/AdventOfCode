using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestCategory("Tree")]
    [TestClass]
    public class Day07Test
    {
        readonly string input = Util.GetInput<Day07>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void TowerBalance_Part1_Regression()
        {
            Assert.AreEqual("aapssr", Day07.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void TowerBalance_Part2_Regression()
        {
            Assert.AreEqual(1458, Day07.Part2(input));
        }
    }
}
