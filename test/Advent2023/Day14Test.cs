using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day14Test
{
    readonly string input = Util.GetInput<Day14>();

    readonly string test = @"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....".Replace("\r", "");

    [TestCategory("Test")]
    [DataTestMethod]
    public void RockRoll_01Test()
    {
        Assert.AreEqual(136, Day14.Part1(test));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void Rotation_Test1()
    {
        string input =
        ".#." + "\n" + // << (1,0)
        "..." + "\n" +
        "..." + "\n";

        string expected1 =
        "..." + "\n" +
        "..#" + "\n" + // << (2,1)
        "..." + "\n";

        string expected2 =
        "..." + "\n" +
        "..." + "\n" +
        ".#." + "\n";

        string expected3 =
        "..." + "\n" +
        "#.." + "\n" +
        "..." + "\n";

        var grid = Util.ParseSparseMatrix<char>(input).Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();

        var grid1 = Day14.RotateGrid(grid, 3);
        var grid1E = Util.ParseSparseMatrix<char>(expected1).Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();

        Assert.IsTrue(grid1.SetEquals(grid1E));

        var grid2 = Day14.RotateGrid(grid1, 3);
        var grid2E = Util.ParseSparseMatrix<char>(expected2).Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();

        Assert.IsTrue(grid2.SetEquals(grid2E));

        var grid3 = Day14.RotateGrid(grid2, 3);
        var grid3E = Util.ParseSparseMatrix<char>(expected3).Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();

        Assert.IsTrue(grid3.SetEquals(grid3E));

        var grid4 = Day14.RotateGrid(grid3, 3);
        Assert.IsTrue(grid4.SetEquals(grid));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void Rotation_Test2()
    {

        string input =
                "###." + "\n" +
                "#..." + "\n" +
                "#..." + "\n" +
                "#..." + "\n";

        string expected1 =
                "####" + "\n" +
                "...#" + "\n" +
                "...#" + "\n" +
                "...." + "\n";

        var grid = Util.ParseSparseMatrix<char>(input).Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();

        var grid1 = Day14.RotateGrid(grid, 4);
        var grid1E = Util.ParseSparseMatrix<char>(expected1).Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();

        Assert.IsTrue(grid1.SetEquals(grid1E));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void RockRoll_02Test()
    {
        Assert.AreEqual(64, Day14.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void ReflectorDish_Part1_Regression()
    {
        Assert.AreEqual(108614, Day14.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void ReflectorDish_Part2_Regression()
    {
        Assert.AreEqual(96447, Day14.Part2(input));
    }
}

