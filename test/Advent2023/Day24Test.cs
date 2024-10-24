using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day24Test
{
    readonly string input = Util.GetInput<Day24>();

    readonly string test = @"19, 13, 30 @ -2,  1, -2
18, 19, 22 @ -1, -1, -2
20, 25, 34 @ -2, -2, -4
12, 31, 28 @ -1, -2, -1
20, 19, 15 @  1, -5, -3".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void _01Test()
    {
        Assert.AreEqual(2, Day24.CheckTestArea(test, 7, 27));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void _02Test()
    {
        Assert.AreEqual(47, Day24.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part1_Regression()
    {
        Assert.AreEqual(29142, Day24.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part2_Regression()
    {
        Assert.AreEqual(848947587263033, Day24.Part2(input));
    }
}

