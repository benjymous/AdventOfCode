using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day17Test
    {
        readonly string input = Util.GetInput<Day17>();
        readonly string test = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Rocktris01Test()
        {
            Assert.AreEqual(3068, Day17.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Rocktris02Test()
        {
            Assert.AreEqual(1514285714288UL, Day17.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Rocktris_Part1_Regression()
        {
            Assert.AreEqual(3059, Day17.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Rocktris_Part2_Regression()
        {
            Assert.AreEqual(0, Day17.Part2(input));
        }
    }
}
