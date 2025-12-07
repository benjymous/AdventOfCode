using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2025.Test;

[TestCategory("2025")]
[TestClass]
public class Day07Test
{
    readonly string input =  Util.GetInput<Day07>();
    readonly string test = ".......S.......\n...............\n.......^.......\n...............\n......^.^......\n...............\n.....^.^.^.....\n...............\n....^.^...^....\n...............\n...^.^...^.^...\n...............\n..^...^.....^..\n...............\n.^.^.^.^.^...^.\n...............";

    [TestCategory("Test")]
    [TestMethod]
    public void Splitters_01Test()
    {
        Assert.AreEqual(21, Day07.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Splitters_02Test()
    {
        Assert.AreEqual(40, Day07.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Teleportation_Part1_Regression()
    {
        Assert.AreEqual(1630, Day07.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Teleportation_Part2_Regression()
    {
        Assert.AreEqual(47857642990160, Day07.Part2(input));
    }
}