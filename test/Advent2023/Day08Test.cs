using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day08Test
{
    readonly string input = Util.GetInput<Day08>();

    [TestCategory("Test")]
    [DataRow("RL\n\nAAA = (BBB, CCC)\nBBB = (DDD, EEE)\nCCC = (ZZZ, GGG)\nDDD = (DDD, DDD)\nEEE = (EEE, EEE)\nGGG = (GGG, GGG)\nZZZ = (ZZZ, ZZZ)", 2)]
    [DataRow("LLR\n\nAAA = (BBB, BBB)\nBBB = (AAA, ZZZ)\nZZZ = (ZZZ, ZZZ)", 6)]
    [DataTestMethod]
    public void GhostMap_01Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day08.Part1(input));
    }

    [TestCategory("Test")]
    [DataRow("LR\n\n11A = (11B, XXX)\n11B = (XXX, 11Z)\n11Z = (11B, XXX)\n22A = (22B, XXX)\n22B = (22C, 22C)\n22C = (22Z, 22Z)\n22Z = (22B, 22B)\nXXX = (XXX, XXX)", 6)]
    [DataTestMethod]
    public void GhostMap_02Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day08.Part2(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void HauntedWasteland_Part1_Regression()
    {
        Assert.AreEqual(12169, Day08.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void HauntedWasteland_Part2_Regression()
    {
        Assert.AreEqual(12030780859469, Day08.Part2(input));
    }
}

