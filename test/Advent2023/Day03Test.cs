using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day03Test
{
    readonly string input =  Util.GetInput<Day03>();

    [TestCategory("Test")]
    [DataRow(@"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..", 4361)]

    [DataRow(        
@"+....
.100.", 100)]

    [DataRow(
@"+...
100.", 100)]

    [DataRow(
@".+..
100.", 100)]

    [DataRow(
@"..+.
100.", 100)]

    [DataRow(
@"...+
100.", 100)]

    [DataRow(
 "@100", 100)]

    [DataRow(
 "100@", 100)]

    [DataRow(
@".100.
+....", 100)]

    [DataRow(
@"100.
+...", 100)]

    [DataRow(
@"100.
.+..", 100)]

    [DataRow(
@"100.
..+.", 100)]

    [DataRow(
@"100.
...+", 100)]

    [TestMethod]
    public void _01Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day03.Part1(input.Replace("\r","")));
    }

    [TestCategory("Test")]
    [DataRow(@"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..", 467835)]
    [DataTestMethod]
    public void _02Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day03.Part2(input.Replace("\r", "")));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part1_Regression()
    {
        Assert.AreEqual(533784, Day03.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part2_Regression()
    {
        Assert.AreEqual(78826761, Day03.Part2(input));
    }
}

