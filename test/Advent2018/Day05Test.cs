using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day05Test
    {
        readonly string input = Util.GetInput<Day05>();

        [TestCategory("Test")]
        [DataRow("aA", 0)]
        [DataRow("abBA", 0)]
        [DataRow("abAB", 4)]
        [DataRow("aabAAB", 6)]
        [DataRow("dabAcCaCBAcCcaDA", 10)]
        [DataTestMethod]
        public void Polymer01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day05.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("dabAcCaCBAcCcaDA", 4)]
        [DataTestMethod]
        public void Polymer02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day05.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Polymer_Part1_Regression()
        {
            Assert.AreEqual(9202, Day05.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Polymer_Part2_Regression()
        {
            Assert.AreEqual(6394, Day05.Part2(input));
        }
    }
}
