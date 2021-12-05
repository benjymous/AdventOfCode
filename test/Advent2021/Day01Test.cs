using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day01Test
    {
        string input = Util.GetInput<Day01>();

        [TestCategory("Test")]
        [DataRow("199,200,208,210,200,207,240,269,260,263", 7)]
        [DataTestMethod]
        public void Depths01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2021.Day01.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("199,200,208,210,200,207,240,269,260,263", 5)]
        [DataTestMethod]
        public void Depths02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2021.Day01.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Depths_Part1_Regression()
        {
            Assert.AreEqual(1655, Day01.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Depths_Part2_Regression()
        {
            Assert.AreEqual(1683, Day01.Part2(input));
        }
    }
}
