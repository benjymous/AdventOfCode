using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day24Test
    {
        readonly string input = Util.GetInput<Day24>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CleaningRobot_Part1_Regression()
        {
            Assert.AreEqual(500, Day24.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CleaningRobot_Part2_Regression()
        {
            Assert.AreEqual(748, Day24.Part2(input));
        }
    }
}
