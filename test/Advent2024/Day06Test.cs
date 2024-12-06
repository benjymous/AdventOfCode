using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day06Test
{
    readonly string input = Util.GetInput<Day06>();
    readonly string test = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void GuardPath_01Test()
    {
        Assert.AreEqual(41, Day06.Part1(test));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void GuardPath_02Test()
    {
        Assert.AreEqual(6, Day06.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void GuardPath_Part1_Regression()
    {
        Assert.AreEqual(5131, Day06.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void GuardPath_Part2_Regression()
    {
        Assert.AreEqual(1784, Day06.Part2(input));
    }
}