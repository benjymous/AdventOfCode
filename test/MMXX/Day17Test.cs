using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXX.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day17Test
    {
        string input = Util.GetInput<Day17>();

        [TestCategory("Test")]
        [DataTestMethod]
        public void Conway1Test()
        {
            Assert.AreEqual(112, Day17.Part1(".#.\n..#\n###"));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Conway2Test()
        {
            Assert.AreEqual(848, Day17.Part2(".#.\n..#\n###"));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Conway_Part1_Regression()
        {
            Assert.AreEqual(207, Day17.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Conway_Part2_Regression()
        {
            Assert.AreEqual(2308, Day17.Part2(input));
        }
    }
}
