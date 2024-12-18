using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day18Test
{
    readonly string input = Util.GetInput<Day18>();
    readonly string test = @"5,4
4,2
4,5
3,0
2,1
6,3
2,4
1,5
0,6
3,3
2,6
5,1
1,2
5,5
2,5
6,5
1,4
0,4
6,4
1,1
6,1
1,0
0,5
1,6
2,0";

    [TestCategory("Test")]
    [DataTestMethod]
    public void MemoryMap_01Test()
    {
        Assert.AreEqual(22, Day18.Part1(test, 6, 6, 12));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void MemoryMap_02Test()
    {
        Assert.AreEqual("6,1", Day18.Part2(test, 6, 6));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void MemoryMap_Part1_Regression()
    {
        Assert.AreEqual(372, Day18.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void MemoryMap_Part2_Regression()
    {
        Assert.AreEqual("25,6", Day18.Part2(input));
    }
}