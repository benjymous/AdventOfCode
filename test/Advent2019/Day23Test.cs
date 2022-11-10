using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestClass]
    public class Day23Test
    {
        readonly string input = Util.GetInput<Day23>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Network_Part1_Regression()
        {
            Assert.AreEqual(17286, Day23.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Network_Part2_Regression()
        {
            Assert.AreEqual(11249, Day23.Part2(input));
        }
    }
}
