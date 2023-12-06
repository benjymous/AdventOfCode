using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day06Test
{
    readonly string input = Util.GetInput<Day06>();
    readonly string test = "Time:      7  15   30\nDistance:  9  40  200";

    [TestCategory("Test")]
    [TestMethod]
    public void BoatRace_01Test()
    {
        Assert.AreEqual(288, Day06.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void BoatRace_02Test()
    {
        Assert.AreEqual(71503, Day06.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void BoatRace_Part1_Regression()
    {
        Assert.AreEqual(440000, Day06.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void BoatRace_Part2_Regression()
    {
        Assert.AreEqual(26187338, Day06.Part2(input));
    }
}

