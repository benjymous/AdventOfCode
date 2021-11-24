using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestCategory("ASCIITerminal")]
    [TestClass]
    public class Day25Test
    {
        string input = Util.GetInput<Day25>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Rescue_Part1_Regression()
        {
            Assert.AreEqual(2098048, Day25.Part1(input));
        }
    }
}
