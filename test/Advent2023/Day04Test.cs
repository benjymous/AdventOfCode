using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day04Test
{
    readonly string input = Util.GetInput<Day04>();

    readonly string test =
@"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void Scratchcards_01Test()
    {
        Assert.AreEqual(13, Day04.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Scratchcards_02Test()
    {
        Assert.AreEqual(30, Day04.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Scratchcards_Part1_Regression()
    {
        Assert.AreEqual(23750, Day04.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Scratchcards_Part2_Regression()
    {
        Assert.AreEqual(13261850, Day04.Part2(input));
    }
}

