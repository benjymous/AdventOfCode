using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestCategory("AStar")]
    [TestCategory("PackedVect")]
    [TestClass]
    public class Day15Test
    {
        readonly string input = Util.GetInput<Day15>();

        readonly string test = @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Chiton01Test()
        {
            Assert.AreEqual(40, Day15.Part1(test.Replace("\r", "")));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Chiton02Test()
        {
            Assert.AreEqual(315, Day15.Part2(test.Replace("\r", "")));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Chiton_Part1_Regression()
        {
            Assert.AreEqual(363, Day15.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Chiton_Part2_Regression()
        {
            Assert.AreEqual(2835, Day15.Part2(input));
        }
    }
}
