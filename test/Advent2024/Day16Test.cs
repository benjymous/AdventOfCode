using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day16Test
{
    readonly string input = Util.GetInput<Day16>();
    const string test1 = @"###############
#.......#....E#
#.#.###.#.###.#
#.....#.#...#.#
#.###.#####.#.#
#.#.#.......#.#
#.#.#####.###.#
#...........#.#
###.#.#####.#.#
#...#.....#.#.#
#.#.#.###.#.#.#
#.....#...#.#.#
#.###.#.#.#.#.#
#S..#.....#...#
###############";

    const string test2 = @"#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################";

    [TestCategory("Test")]
    [DataRow(test1, 7036)]
    [DataRow(test2, 11048)]
    [DataTestMethod]
    public void ReindeerMaze_01Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day16.Part1(input));
    }

    [TestCategory("Test")]
    [DataRow(test1, 45)]
    [DataRow(test2, 64)]
    [DataTestMethod]
    public void ReindeerMaze_02Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day16.Part2(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void ReindeerMaze_Part1_Regression()
    {
        Assert.AreEqual(134588, Day16.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void ReindeerMaze_Part2_Regression()
    {
        Assert.AreEqual(631, Day16.Part2(input));
    }
}