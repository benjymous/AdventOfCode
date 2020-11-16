using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXIX.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestCategory("ASCIITerminal")]
    [TestClass]
    public class Day17Test
    {
        string input = Util.GetInput<Day17>();

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
