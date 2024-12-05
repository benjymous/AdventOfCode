using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day05Test
{
    readonly string input = Util.GetInput<Day05>();
    readonly string test = @"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void PageOrder_01Test()
    {
        Assert.AreEqual(143, Day05.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void PageOrder_02Test()
    {
        Assert.AreEqual(123, Day05.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void PrintQueue_Part1_Regression()
    {
        Assert.AreEqual(6034, Day05.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void PrintQueue_Part2_Regression()
    {
        Assert.AreEqual(6305, Day05.Part2(input));
    }
}