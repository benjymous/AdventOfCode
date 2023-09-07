using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestCategory("AStar")]
    [TestClass]
    public class Day12Test
    {
        readonly string input = Util.GetInput<Day12>();
        readonly string test = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        public void Climbing01Test()
        {
            Assert.AreEqual(31, Day12.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Climbing02Test()
        {
            Assert.AreEqual(29, Day12.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Climbing_Part1_Regression()
        {
            Assert.AreEqual(462, Day12.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Climbing_Part2_Regression()
        {
            Assert.AreEqual(451, Day12.Part2(input));
        }
    }
}
