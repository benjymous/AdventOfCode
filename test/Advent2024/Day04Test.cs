using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day04Test
{
    readonly string input = Util.GetInput<Day04>();
    readonly string test = @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX";

    [TestCategory("Test")]
    [TestMethod]
    public void Wordsearch_01Test()
    {
        Assert.AreEqual(18, Day04.Part1(test));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void Wordsearch_02Test()
    {
        Assert.AreEqual(9, Day04.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Wordsearch_Part1_Regression()
    {
        Assert.AreEqual(2662, Day04.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Wordsearch_Part2_Regression()
    {
        Assert.AreEqual(2034, Day04.Part2(input));
    }
}