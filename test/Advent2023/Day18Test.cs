using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day18Test
{
    readonly string input = ""; // Util.GetInput<Day18>();

    [TestCategory("Test")]
    [DataRow("???", 0)]
    [DataTestMethod]
    public void _01Test(string input, int expected)
    {
        Assert.IsTrue(Day18.Part1(input) == expected);
    }

    [TestCategory("Test")]
    [DataRow("???", 0)]
    [DataTestMethod]
    public void _02Test(string input, int expected)
    {
        Assert.IsTrue(Day18.Part2(input) == expected);
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part1_Regression()
    {
        Assert.AreEqual(0, Day18.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part2_Regression()
    {
        Assert.AreEqual(0, Day18.Part2(input));
    }
}

