using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day22Test
{
    readonly string input = Util.GetInput<Day22>();

    readonly string test = "1,0,1~1,2,1\n0,0,2~2,0,2\n0,2,3~2,2,3\n0,0,4~0,2,4\n2,0,5~2,2,5\n0,1,6~2,1,6\n1,1,8~1,1,9";

    [TestCategory("Test")]
    [TestMethod]
    public void Bricks_01Test()
    {
        Assert.AreEqual(5, Day22.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Bricks_02Test()
    {
        Assert.AreEqual(7, Day22.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void SandSlabs_Part1_Regression()
    {
        Assert.AreEqual(418, Day22.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void SandSlabs_Part2_Regression()
    {
        Assert.AreEqual(70702, Day22.Part2(input));
    }
}