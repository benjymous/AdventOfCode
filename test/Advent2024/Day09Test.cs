using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day09Test
{
    readonly string input = Util.GetInput<Day09>();
    readonly string test = "2333133121414131402";

    [TestCategory("Test")]
    [TestMethod]
    public void Fragment_01Test()
    {
        Assert.AreEqual(1928, Day09.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Fragment_02Test()
    {
        Assert.AreEqual(2858, Day09.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Fragment_Part1_Regression()
    {
        Assert.AreEqual(6471961544878, Day09.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Fragment_Part2_Regression()
    {
        Assert.AreEqual(6511178035564, Day09.Part2(input));
    }
}