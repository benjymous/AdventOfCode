using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day20Test
{
    readonly string input = Util.GetInput<Day20>();
    readonly string test = @"###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############".Replace("\r", "");

    [TestCategory("Test")]
    [DataTestMethod]
    public void CheatFinder_01Test()
    {
        Assert.AreEqual(44, Day20.FindCheats(test, 2, 1));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void CheatFinder_02Test()
    {
        Assert.AreEqual(285, Day20.FindCheats(test, 20, 50));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Race_Part1_Regression()
    {
        Assert.AreEqual(1381, Day20.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Race_Part2_Regression()
    {
        Assert.AreEqual(982124, Day20.Part2(input));
    }
}