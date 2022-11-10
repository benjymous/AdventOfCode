using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestCategory("ManhattanVector")]
    [TestClass]
    public class Day13Test
    {
        readonly string input = Util.GetInput<Day13>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Pong_Part1_Regression()
        {
            Assert.AreEqual(230, Day13.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Pong_Part2_Regression()
        {
            Assert.AreEqual(11140, Day13.Part2(input));
        }

    }
}
