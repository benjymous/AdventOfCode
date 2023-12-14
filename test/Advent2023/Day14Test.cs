using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day14Test
{
    readonly string input = Util.GetInput<Day14>();

    readonly string test = @"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....".Replace("\r", "");

    [TestCategory("Test")]
    [DataTestMethod]
    public void RockRoll_01Test()
    {
        Assert.AreEqual(136, Day14.Part1(test));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void RockRoll_02Test()
    {
        Assert.AreEqual(64, Day14.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void ReflectorDish_Part1_Regression()
    {
        Assert.AreEqual(108614, Day14.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void ReflectorDish_Part2_Regression()
    {
        Assert.AreEqual(0, Day14.Part2(input));
    }
}

