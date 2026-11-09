using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2026.Test;

[TestCategory("2026")]
[TestClass]
public class Day08Test
{
    readonly string input = ""; // Util.GetInput<Day08>();

    [TestCategory("Test")]
    [DataRow("???", 0)]
    [TestMethod]
    public void _01Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day08.Part1(input));
    }

    [TestCategory("Test")]
    [DataRow("???", 0)]
    [TestMethod]
    public void _02Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day08.Part2(input));
    }

    [TestCategory("Regression")]
    [TestMethod]
    public void _Part1_Regression()
    {
        Assert.AreEqual(0, Day08.Part1(input));
    }

    [TestCategory("Regression")]
    [TestMethod]
    public void _Part2_Regression()
    {
        Assert.AreEqual(0, Day08.Part2(input));
    }
}