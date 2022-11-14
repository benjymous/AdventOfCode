using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day20Test
    {
        readonly string input = Util.GetInput<Day20>();

        readonly string test = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

#..#.
#....
##..#
..#..
..###".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        public void Enhancement1Test()
        {
            Assert.AreEqual(35, Day20.Simulate(test, 2));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Enhancement2Test()
        {
            Assert.AreEqual(3351, Day20.Simulate(test, 50));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Enhancement_Part1_Regression()
        {
            Assert.AreEqual(5339, Day20.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Enhancement_Part2_Regression()
        {
            Assert.AreEqual(18395, Day20.Part2(input));
        }
    }
}
