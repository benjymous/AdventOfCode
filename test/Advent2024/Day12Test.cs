using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day12Test
{
    readonly string input = Util.GetInput<Day12>();
    readonly string test = @"RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE".Replace("\r", "");

    readonly string test2 = @"EEEEE
EXXXX
EEEEE
EXXXX
EEEEE".Replace("\r", "");

    readonly string test3 = @"AAAAAA
AAABBA
AAABBA
ABBAAA
ABBAAA
AAAAAA".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void Fencing_01Test()
    {
        Assert.AreEqual(1930, Day12.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Fencing_02Test()
    {
        Assert.AreEqual(1206, Day12.Part2(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Fencing_02aTest()
    {
        Assert.AreEqual(236, Day12.Part2(test2));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Fencing_02bTest()
    {
        Assert.AreEqual(368, Day12.Part2(test3));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Fencing_Part1_Regression()
    {
        Assert.AreEqual(1415378, Day12.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Fencing_Part2_Regression()
    {
        Assert.AreEqual(862714, Day12.Part2(input));
    }
}