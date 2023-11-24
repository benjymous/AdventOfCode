using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestCategory("ManhattanVector")]
    [TestClass]
    public class Day03Test
    {
        readonly string input = Util.GetInput<Day03>();

        [TestCategory("Test")]
        [DataRow("1", 0)]
        [DataRow("12", 3)]
        [DataRow("23", 2)]
        [DataRow("1024", 31)]
        [DataTestMethod]
        public void Spiral01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day03.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("1", 2)]
        [DataRow("4", 5)]
        [DataRow("23", 25)]
        [DataRow("750", 806)]
        [DataTestMethod]
        public void Spiral02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day03.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Spiral_Part1_Regression()
        {
            Assert.AreEqual(326, Day03.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Spiral_Part2_Regression()
        {
            Assert.AreEqual(363010, Day03.Part2(input));
        }
    }
}
