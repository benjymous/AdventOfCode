using AoC.Utils.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2025.Test;

[TestCategory("2025")]
[TestClass]
public class Day09Test
{
    readonly string input = Util.GetInput<Day09>();
    readonly string test = "7,1\n11,1\n11,7\n9,7\n9,5\n2,5\n2,3\n7,3";

    [TestCategory("Test")]
    [TestMethod]
    public void TileSize_01Test()
    {
        Assert.AreEqual(50, Day09.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void TileSize_02Test()
    {
        Assert.AreEqual(24, Day09.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void TileFlipping_Part1_Regression()
    {
        Assert.AreEqual(4749838800, Day09.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void TileFlipping_Part2_Regression()
    {
        Assert.AreEqual(1624057680, Day09.Part2(input));
    }
}