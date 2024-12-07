using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day07Test
{
    readonly string input = Util.GetInput<Day07>();
    readonly string test = @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20".Replace("\r","");

    [TestCategory("Test")]
    [TestMethod]
    public void Operators_01Test()
    {
        Assert.AreEqual(3749, Day07.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Operators_02Test()
    {
        Assert.AreEqual(11387, Day07.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void BridgeMaths_Part1_Regression()
    {
        Assert.AreEqual(66343330034722, Day07.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void BridgeMaths_Part2_Regression()
    {
        Assert.AreEqual(637696070419031, Day07.Part2(input));
    }
}