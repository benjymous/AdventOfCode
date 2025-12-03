using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2025.Test;

[TestCategory("2025")]
[TestClass]
public class Day03Test
{
    readonly string input = Util.GetInput<Day03>();
    string test = "987654321111111\n811111111111119\n234234234234278\n818181911112111";

    [TestCategory("Test")]
    [TestMethod]
    public void Joltage_01Test()
    {
        Assert.AreEqual(357, Day03.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Joltage_02Test()
    {
        Assert.AreEqual(3121910778619, Day03.Part2(test));
    }

    [TestCategory("Regression")]
    [TestMethod]
    public void Lobby_Part1_Regression()
    {
        Assert.AreEqual(17311, Day03.Part1(input));
    }

    [TestCategory("Regression")]
    [TestMethod]
    public void Lobby_Part2_Regression()
    {
        Assert.AreEqual(171419245422055, Day03.Part2(input));
    }
}