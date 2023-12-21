using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day21Test
{
    readonly string input = Util.GetInput<Day21>();
    readonly string test = @"...........
.....###.#.
.###.##..#.
..#.#...#..
....#.#....
.##..S####.
.##..#...#.
.......##..
.##.#.####.
.##..##.##.
...........".Replace("\r", "");

    readonly string test2 = @"...\n.S.\n...";

    [TestCategory("Test")]
    [TestMethod]
    public void _01Test()
    {
        Assert.AreEqual(16, Day21.CountSteps(test, 6));
    }

    [TestCategory("Test")]
    [DataRow(6, 16)]
    [DataRow(10, 50)]
    [DataRow(50, 1594)]
    [DataRow(100, 6536)]
    [DataRow(500, 167004)]
    [DataRow(1000, 668697)]
    [DataRow(5000, 16733044)]
    [DataTestMethod]
    public void _02Test(int input, int expected)
    {
        Assert.AreEqual(expected, Day21.SolvePart2(test, input));
    }

    [TestCategory("Test")]
    [DataRow(1, 4)]
    [DataRow(10, 0)]
    [DataRow(50, 0)]
    [DataRow(100, 0)]
    [DataTestMethod]
    public void _02aTest(int input, int expected)
    {
        Assert.AreEqual(expected, Day21.SolvePart2(test2, input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part1_Regression()
    {
        Assert.AreEqual(3682, Day21.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part2_Regression()
    {
        Assert.AreEqual(0, Day21.Part2(input));
    }
}

