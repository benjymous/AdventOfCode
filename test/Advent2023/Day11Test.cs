using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day11Test
{
    readonly string input = Util.GetInput<Day11>();

    readonly string test = @"...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....".Replace("\r\n", "\n");

    [TestCategory("Test")]
    [DataTestMethod]
    public void GalaxyExpansion_01Test()
    {
        Assert.AreEqual(374, Day11.Part1(test));
    }

    [TestCategory("Test")]
    [DataRow(10, 1030)]
    [DataRow(100, 8410)]
    [DataTestMethod]
    public void GalaxyExpansion_02Test(int expansion, int expected)
    {
        Assert.AreEqual(expected, Day11.MeasureGalaxies(test, expansion));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void CosmicExpansion_Part1_Regression()
    {
        Assert.AreEqual(9445168, Day11.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void CosmicExpansion_Part2_Regression()
    {
        Assert.AreEqual(742305960572, Day11.Part2(input));
    }
}

