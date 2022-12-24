using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day22Test
    {
        readonly string input = Util.GetInput<Day22>();
        readonly string test = @"        ...#
        .#..
        #...
        ....
...#.......#
........#...
..#....#....
..........#.
        ...#....
        .....#..
        .#......
        ......#.

10R5L5R10L4R5L5".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        public void CubeMap01Test()
        {
            Assert.AreEqual(6032, Day22.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void CubeMap02Test()
        {
            Assert.AreEqual(5031, Day22.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CubeMap_Part1_Regression()
        {
            Assert.AreEqual(162186, Day22.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CubeMap_Part2_Regression()
        {
            Assert.AreEqual(55267, Day22.Part2(input));
        }
    }
}
