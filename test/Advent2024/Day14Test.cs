using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day14Test
{
    readonly string input = Util.GetInput<Day14>();
    readonly string test = @"p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3";

    [TestCategory("Test")]
    [TestMethod]
    public void GuardQuadrants_01Test()
    {
        int width = 11;
        int height = 7;

        var r = Day14.SolvePt1(test, width, height);

        Assert.AreEqual(12, r);
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void GuardWatching_Part1_Regression()
    {
        Assert.AreEqual(222901875, Day14.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void GuardWatching_Part2_Regression()
    {
        Assert.AreEqual(6243, Day14.Part2(input));
    }
}