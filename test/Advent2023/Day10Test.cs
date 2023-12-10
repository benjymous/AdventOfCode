using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day10Test
{
    readonly string input = Util.GetInput<Day10>();


    readonly string test1 = @"...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........".Replace("\r","");

    [TestCategory("Test")]
    [TestMethod]
    public void Pipes_01Test()
    {
        Assert.AreEqual(4, Day10.Part2(test1));
    }

    readonly string test2 = @".F----7F7F7F7F-7....
.|F--7||||||||FJ....
.||.FJ||||||||L7....
FJL7L7LJLJ||LJ.L-7..
L--J.L7...LJS7F-7L7.
....F-J..F7FJ|L7L7L7
....L7.F7||L7|.L7L7|
.....|FJLJ|FJ|F7|.LJ
....FJL-7.||.||||...
....L---J.LJ.LJLJ...".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void Pipes_02Test()
    {
        Assert.AreEqual(8, Day10.Part2(test2));
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

