using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestCategory("FloodFill")]
    [TestCategory("PackedVect")]
    [TestClass]
    public class Day14Test
    {
        readonly string input = Util.GetInput<Day14>();

        [TestMethod]
        public void Defrag01Test()
        {
            Assert.AreEqual(8108, Day14.Part1("flqrgnkx"));
        }

        [TestMethod]
        public void Defrag02Test()
        {
            Assert.AreEqual(1242, Day14.Part2("flqrgnkx"));
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void Defrag_Part1_Regression()
        {
            Assert.AreEqual(8106, Day14.Part1(input));
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void Defrag_Part2_Regression()
        {
            Assert.AreEqual(1164, Day14.Part2(input));
        }
    }
}
