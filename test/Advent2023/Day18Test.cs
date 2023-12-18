using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day18Test
{
    readonly string input = Util.GetInput<Day18>();

    readonly string test = @"R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 2 (#015232)
U 2 (#7a21e3)".Replace("\r", "");

    [TestCategory("Test")]
    [DataTestMethod]
    public void Lagoon_01Test()
    {
        Assert.AreEqual(62, Day18.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Lagoon_02Test()
    {
        Assert.AreEqual(952408144115, Day18.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void LavaductLagoon_Part1_Regression()
    {
        Assert.AreEqual(67891, Day18.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void LavaductLagoon_Part2_Regression()
    {
        Assert.AreEqual(94116351948493, Day18.Part2(input));
    }
}

