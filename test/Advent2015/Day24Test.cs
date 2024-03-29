using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day24Test
    {
        readonly string input = Util.GetInput<Day24>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Balance_Part1_Regression()
        {
            Assert.AreEqual(11266889531, Day24.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Balance_Part2_Regression()
        {
            Assert.AreEqual(77387711, Day24.Part2(input));
        }
    }
}
