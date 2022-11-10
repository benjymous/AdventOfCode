using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day22Test
    {
        readonly string input = Util.GetInput<Day22>();
        readonly string test =
            "Player 1:\n" +
            "9\n" +
            "2\n" +
            "6\n" +
            "3\n" +
            "1\n" +
            "\n" +
            "Player 2:\n" +
            "5\n" +
            "8\n" +
            "4\n" +
            "7\n" +
            "10";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Combat1Test()
        {
            Assert.AreEqual(306, Day22.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Combat2Test()
        {
            Assert.AreEqual(291, Day22.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Combat_Part1_Regression()
        {
            Assert.AreEqual(29764, Day22.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Combat_Part2_Regression()
        {
            Assert.AreEqual(32588, Day22.Part2(input));
        }
    }
}
