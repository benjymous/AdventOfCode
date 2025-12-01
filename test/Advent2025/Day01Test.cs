using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2025.Test;

[TestCategory("2025")]
[TestClass]
public class Day01Test
{
    readonly string input = Util.GetInput<Day01>();

    readonly string test = "L68\nL30\nR48\nL5\nR60\nL55\nL1\nL99\nR14\nL82";

    [TestCategory("Test")]
    [TestMethod]
    public void CombinationLock_01Test()
    {
        Assert.AreEqual(3, Day01.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void CombinationLock_02Test()
    {
        Assert.AreEqual(6, Day01.Part2(test));
    }

    [TestCategory("Regression")]
    [TestMethod]
    public void CombinationLock_Part1_Regression()
    {
        Assert.AreEqual(995, Day01.Part1(input));
    }

    [TestCategory("Regression")]
    [TestMethod]
    public void CombinationLock_Part2_Regression()
    {
        Assert.AreEqual(5847, Day01.Part2(input));
    }
}