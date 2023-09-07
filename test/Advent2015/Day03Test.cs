using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day03Test
    {
        readonly string input = Util.GetInput<Day03>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void SantaStepper_Part1_Regression()
        {
            Assert.AreEqual(2081, Day03.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void SantaStepper_Part2_Regression()
        {
            Assert.AreEqual(2341, Day03.Part2(input));
        }
    }
}
