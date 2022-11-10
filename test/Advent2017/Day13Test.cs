using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day13Test
    {
        readonly string input = Util.GetInput<Day13>();

        [TestCategory("Test")]
        [DataRow("0: 3\n1: 2\n4: 4\n6: 4", 24)]
        [DataTestMethod]
        public void Scanner01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2017.Day13.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("0: 3\n1: 2\n4: 4\n6: 4", 10)]
        [DataTestMethod]
        public void Scanner02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2017.Day13.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Scanner_Part1_Regression()
        {
            Assert.AreEqual(1504, Advent2017.Day13.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Scanner_Part2_Regression()
        {
            Assert.AreEqual(3823370, Advent2017.Day13.Part2(input));
        }

    }
}
