using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day08Test
{
    readonly string input = Util.GetInput<Day08>();
    readonly string test = @"............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void NodePositions_01Test()
    {
        Assert.AreEqual(14, Day08.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void NodePositions_02Test()
    {
        Assert.AreEqual(34, Day08.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void NodePositions_Part1_Regression()
    {
        Assert.AreEqual(398, Day08.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void NodePositions_Part2_Regression()
    {
        Assert.AreEqual(1333, Day08.Part2(input));
    }
}