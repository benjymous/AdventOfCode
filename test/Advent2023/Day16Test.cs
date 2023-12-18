using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day16Test
{
    readonly string input = Util.GetInput<Day16>();

    readonly string test = @".|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void Lava_01Test()
    {
        Assert.AreEqual(46, Day16.Part1(test));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void Lava_02Test()
    {
        Assert.AreEqual(51, Day16.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Lava_Part1_Regression()
    {
        Assert.AreEqual(6361, Day16.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Lava_Part2_Regression()
    {
        Assert.AreEqual(6701, Day16.Part2(input));
    }
}

