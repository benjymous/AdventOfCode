using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day01Test
{
    readonly string input = Util.GetInput<Day01>();

    string test = "3   4\n4   3\n2   5\n1   3\n3   9\n3   3";

    [TestCategory("Test")]
    [TestMethod]
    public void Historian_01Test()
    {
        Assert.AreEqual(11, Day01.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Historian_02Test()
    {
        Assert.AreEqual(31, Day01.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Historian_Part1_Regression()
    {
        Assert.AreEqual(1830467, Day01.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Historian_Part2_Regression()
    {
        Assert.AreEqual(26674158, Day01.Part2(input));
    }
}