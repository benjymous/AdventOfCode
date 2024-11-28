using AoC.Utils.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day12Test
{
    readonly string input = Util.GetInput<Day12>();

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
    [DataRow("?? 1,1", 0)]
    [DataRow("??? 1,1", 1)] // #.#
    [DataRow("?.? 1,1", 1)] // #.#
    [DataRow("#.? 1,1", 1)] // #.#
    [DataRow("?.# 1,1", 1)] // #.#
    [DataRow("#?? 1,1", 1)] // #.#
    [DataRow("??# 1,1", 1)] // #.#
    [DataRow("?#? 1,1", 0)]
    [DataRow("?? 1", 2)] // .#  #.
    [DataRow(".?? 1", 2)] // ..#  .#.
    [DataRow("???? 1,1", 3)] // #.#.  #..#  .#.#  
    [DataRow("???? 1,2", 1)] // #.##   
    [DataRow("???.### 1,1,3", 1)]
    [DataRow(".??..??...?##. 1,1,3", 4)]
    [DataRow(".??..??... 1,1", 4)]
    [DataRow(".??. 1", 2)]
    [DataRow("??..?? 1,1", 4)]
    [DataRow("?#?#?#?#?#?#?#? 1,3,1,6", 1)]
    [DataRow("????.#...#... 4,1,1", 1)]
    [DataRow("????.######..#####. 1,6,5", 4)]
    [DataRow("?###???????? 3,2,1", 10)]
    [DataRow("??????? 2,1", 10)]
    [DataRow("#?? 2", 1)]
    [DataTestMethod]
    public void Springs01aTest(string input, int expected)
    {
        Assert.AreEqual(expected, Solve1Single(input));
    }

    static long Solve1Single(string input)
    {
        var row = Parser.FromString<Day12.Row>(input);
        return Day12.Solve(row.Input, row.Lengths);
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
    public void HotSprings_Part1_Regression()
    {
        Assert.AreEqual(7771, Day12.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void HotSprings_Part2_Regression()
    {
        Assert.AreEqual(10861030975833, Day12.Part2(input));
    }
}

