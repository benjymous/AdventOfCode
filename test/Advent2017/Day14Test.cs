using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestCategory("FloodFill")]
    [TestCategory("AStar")]
    [TestClass]
    public class Day14Test
    {
        readonly string input = Util.GetInput<Day14>();

        [DataTestMethod]
        public void Defrag01Test()
        {
            Assert.AreEqual(8108, Advent2017.Day14.Part1("flqrgnkx"));
        }

        [DataTestMethod]
        public void Defrag02Test()
        {
            Assert.AreEqual(1242, Advent2017.Day14.Part2("flqrgnkx"));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Defrag_Part1_Regression()
        {
            Assert.AreEqual(8106, Advent2017.Day14.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Defrag_Part2_Regression()
        {
            Assert.AreEqual(1164, Advent2017.Day14.Part2(input));
        }

    }
}
