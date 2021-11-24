using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestClass]
    public class Day19Test
    {
        string input = Util.GetInput<Day19>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void TractorBeam_Part1_Regression()
        {
            Assert.AreEqual(129, Day19.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void TractorBeam_Part2_Regression()
        {
            Assert.AreEqual(14040699, Day19.Part2(input));
        }
    }
}
