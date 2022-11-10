using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestCategory("ASCIITerminal")]
    [TestClass]
    public class Day21Test
    {
        readonly string input = Util.GetInput<Day21>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Springbot_Part1_Regression()
        {
            Assert.AreEqual(19359969, Day21.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Springbot_Part2_Regression()
        {
            Assert.AreEqual(1140082748, Day21.Part2(input));
        }
    }
}
