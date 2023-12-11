using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day10Test
{
    readonly string input = Util.GetInput<Day10>();

    const string test1 = ".....\n.S-7.\n.|.|.\n.L-J.\n.....";
    const string test2 = "..F7.\n.FJ|.\nSJ.L7\n|F--J\nLJ...";
    const string test3 = @"...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........";

    const string test4 = @".F----7F7F7F7F-7....
.|F--7||||||||FJ....
.||.FJ||||||||L7....
FJL7L7LJLJ||LJ.L-7..
L--J.L7...LJS7F-7L7.
....F-J..F7FJ|L7L7L7
....L7.F7||L7|.L7L7|
.....|FJLJ|FJ|F7|.LJ
....FJL-7.||.||||...
....L---J.LJ.LJLJ...";

    [TestCategory("Test")]
    [DataRow(test1, 4)]
    [DataRow(test2, 8)]
    [DataRow(test3, 23)]
    [DataRow(test4, 70)]
    [DataTestMethod]
    public void PipeLength_Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day10.Part1(input.Replace("\r", "")));
    }

    [TestCategory("Test")]
    [DataRow(test1, 1)]
    [DataRow(test2, 1)]
    [DataRow(test3, 4)]
    [DataRow(test4, 8)]
    [DataTestMethod]
    public void PipesInner_Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day10.Part2(input.Replace("\r", "")));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void PipeMaze_Part1_Regression()
    {
        Assert.AreEqual(6800, Day10.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void PipeMaze_Part2_Regression()
    {
        Assert.AreEqual(483, Day10.Part2(input));
    }
}

