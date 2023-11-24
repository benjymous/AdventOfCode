using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day16Test
    {
        readonly string input = Util.GetInput<Day16>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void DragonChecksum_Part1_Regression()
        {
            Assert.AreEqual("10010110010011110", Day16.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void DragonChecksum_Part2_Regression()
        {
            Assert.AreEqual("01101011101100011", Day16.Part2(input));
        }
    }
}
