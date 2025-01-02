using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day25Test
{
    readonly string input = Util.GetInput<Day25>();
    readonly string test = @"#####
.####
.####
.####
.#.#.
.#...
.....

#####
##.##
.#.##
...##
...#.
...#.
.....

.....
#....
#....
#...#
#.#.#
#.###
#####

.....
.....
#.#..
###..
###.#
###.#
#####

.....
.....
.....
#....
#.#..
#.#.#
#####";

    [TestCategory("Test")]
    [TestMethod]
    public void _01Test()
    {
        Assert.AreEqual(3, Day25.Part1(test.Replace("\r","")));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part1_Regression()
    {
        Assert.AreEqual(3397, Day25.Part1(input));
    }
}