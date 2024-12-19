using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day19Test
{
    readonly string input = Util.GetInput<Day19>();
    readonly string test = @"r, wr, b, g, bwu, rb, gb, br

brwrr
bggr
gbbr
rrbgbr
ubwu
bwurrg
brgr
bbrgwb".Replace("\r", "");

    [TestCategory("Test")]
    [DataTestMethod]
    public void Towels_01Test()
    {
        Assert.AreEqual(6, Day19.Part1(test));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void Towels_02Test()
    {
        Assert.AreEqual(16, Day19.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void TowelPatterns_Part1_Regression()
    {
        Assert.AreEqual(313, Day19.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void TowelPatterns_Part2_Regression()
    {
        Assert.AreEqual(666491493769758, Day19.Part2(input));
    }
}