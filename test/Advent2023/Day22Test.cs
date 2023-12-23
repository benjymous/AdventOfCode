using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day22Test
{
    readonly string input = Util.GetInput<Day22>();

    readonly string test = "1,0,1~1,2,1\n0,0,2~2,0,2\n0,2,3~2,2,3\n0,0,4~0,2,4\n2,0,5~2,2,5\n0,1,6~2,1,6\n1,1,8~1,1,9";

    [TestCategory("Test")]
    [TestMethod]
    public void _01Test()
    {
        Assert.AreEqual(5, Day22.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void _02Test()
    {
        Assert.AreEqual(7, Day22.Part2(test));
    }

 /*
    G
    |
    F
   /\    
   D E
  / X \
  B   C
   \ /
    A
*/

    [TestCategory("Test")]
    [DataTestMethod]
    [DataRow("B", "A", true)]
    [DataRow("A", "B", false)]
    [DataRow("G", "F", true)]
    [DataRow("C", "A", true)]
    [DataRow("D", "A", true)]
    [DataRow("E", "A", true)]
    [DataRow("F", "A", true)]
    [DataRow("G", "A", true)]
    [DataRow("D", "E", false)]
    [DataRow("B", "C", false)]
    public void SupportedTest(string brick, string supportedBy, bool expected)
    {
        var bricks = Day22.SimulateBricks(test);
        var dict = bricks.ToDictionary(b => b.Name, b => b);

        Assert.AreEqual(expected, Day22.IsSupportedBy(dict[brick], dict[supportedBy]));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part1_Regression()
    {
        Assert.AreEqual(418, Day22.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part2_Regression()
    {
        Assert.AreEqual(0, Day22.Part2(input));
    }
}

