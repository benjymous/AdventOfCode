using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestCategory("AStar")]
    [TestClass]
    public class Day13Test
    {
        readonly string input = Util.GetInput<Day13>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CubicleMaze_Part1_Regression()
        {
            Assert.AreEqual(86, Day13.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CubicleMaze_Part2_Regression()
        {
            Assert.AreEqual(127, Day13.Part2(input));
        }
    }
}
