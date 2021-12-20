using AoC.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static AoC.Advent2021.Day20;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day20Test
    {
        string input = Util.GetInput<Day20>();

        string test = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

#..#.
#....
##..#
..#..
..###".Replace("\r", "");

        [DataTestMethod]
        public void Enhancement1Test()
        {
            Assert.AreEqual(35, Day20.Simulate(test, 2));
        }

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
