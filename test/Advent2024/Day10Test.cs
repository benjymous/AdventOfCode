using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day10Test
{
    readonly string input = Util.GetInput<Day10>();
    readonly string test = @"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void Trails_01Test()
    {
        Assert.AreEqual(36, Day10.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Trails_02Test()
    {
        Assert.AreEqual(81, Day10.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void HikingTrails_Part1_Regression()
    {
        Assert.AreEqual(816, Day10.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void HikingTrails_Part2_Regression()
    {
        Assert.AreEqual(1960, Day10.Part2(input));
    }
}