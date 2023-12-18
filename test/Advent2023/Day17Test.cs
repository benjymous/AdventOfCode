using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day17Test
{
    readonly string input = Util.GetInput<Day17>();

    readonly string test = @"2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533".Replace("\r", "");

    readonly string test2 = @"111111111111
999999999991
999999999991
999999999991
999999999991".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void Crucible_01Test()
    {
        Assert.AreEqual(102, Day17.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Crucible_02Test()
    {
        Assert.AreEqual(94, Day17.Part2(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Crucible_02aTest()
    {
        Assert.AreEqual(71, Day17.Part2(test2));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Crucible_Part1_Regression()
    {
        Assert.AreEqual(771, Day17.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Crucible_Part2_Regression()
    {
        Assert.AreEqual(930, Day17.Part2(input));
    }
}