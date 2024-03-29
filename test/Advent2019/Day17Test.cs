using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestCategory("ASCIITerminal")]
    [TestCategory("ManhattanVector")]
    [TestCategory("Direction")]
    [TestClass]
    public class Day17Test
    {
        readonly string input = Util.GetInput<Day17>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Hoovamatic_Part1_Regression()
        {
            Assert.AreEqual(5056, Day17.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Hoovamatic_Part2_Regression()
        {
            Assert.AreEqual(942367, Day17.Part2(input));
        }
    }
}
