using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day12Test
{
    readonly string input = Util.GetInput<Day12>();

    [TestCategory("Test")]
    [DataRow("#.#.###", new int[] { 1, 1, 3 })]
    [DataRow(".#...#....###.", new int[] { 1, 1, 3 })]
    [DataRow(".#.###.#.######", new int[] { 1, 3, 1, 6 })]
    [DataRow("####.#...#...", new int[] { 4, 1, 1 })]
    [DataRow("#....######..#####.", new int[] { 1, 6, 5 })]
    [DataRow(".###.##....#", new int[] { 3, 2, 1 })]
    [DataTestMethod]
    public void SpringCount_Test(string input, int[] expected)
    {
        var count = Day12.CountSprings(input.Select(c => c == '#').ToArray());
        Assert.IsTrue(expected.SequenceEqual(count));
    }

    readonly string test = @"???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1";

    readonly string test1 = @"???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5";

    [TestCategory("Test")]
    [TestMethod]
    public void Springs01Test()
    {
        Assert.AreEqual(21, Day12.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Springs02Test()
    {
        Assert.AreEqual(525152, Day12.Part2(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Springs02aTest()
    {
        Assert.AreEqual(18902, Day12.Part2(test1));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part1_Regression()
    {
        Assert.AreEqual(7771, Day12.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part2_Regression()
    {
        Assert.AreEqual(0, Day12.Part2(input));
    }
}

