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

    [TestCategory("Test")]
    [TestMethod]
    public void _01Test()
    {
        Assert.AreEqual(16, Day21.Part1(test));
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
        Assert.AreEqual(609012263058042, Day21.Part2(input));
    }
}

