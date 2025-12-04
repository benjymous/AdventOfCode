using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2025.Test;

[TestCategory("2025")]
[TestClass]
public class Day04Test
{
    readonly string input = Util.GetInput<Day04>();
    readonly string test = "..@@.@@@@.\n@@@.@.@.@@\n@@@@@.@.@@\n@.@@@@..@.\n@@.@@@@.@@\n.@@@@@@@.@\n.@.@.@.@@@\n@.@@@.@@@@\n.@@@@@@@@.\n@.@.@@@.@.";

    [TestCategory("Test")]
    [TestMethod]
    public void Rolls_01Test()
    {
        Assert.AreEqual(13, Day04.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Rolls_02Test()
    {
        Assert.AreEqual(43, Day04.Part2(test));
    }

    [TestCategory("Regression")]
    [TestMethod]
    public void Forklifts_Part1_Regression()
    {
        Assert.AreEqual(1464, Day04.Part1(input));
    }

    [TestCategory("Regression")]
    [TestMethod]
    public void Forklifts_Part2_Regression()
    {
        Assert.AreEqual(8409, Day04.Part2(input));
    }
}