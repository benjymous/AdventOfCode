using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day09Test
{
    readonly string input = Util.GetInput<Day09>();

    [TestCategory("Test")]
    [DataRow("0 3 6 9 12 15", 18)]
    [DataRow("1 3 6 10 15 21", 28)]
    [DataRow("10 13 16 21 30 45", 68)]
    [DataTestMethod]
    public void GetNextTest(string input, int expected)
    {
        var nums = Util.ExtractNumbers<int>(input).ToArray();
        Assert.AreEqual(expected, Day09.GetNext(nums));
    }

    [TestCategory("Test")]
    [DataRow("0 3 6 9 12 15", -3)]
    [DataRow("1 3 6 10 15 21", 0)]
    [DataRow("10 13 16 21 30 45", 5)]
    [DataTestMethod]
    public void GetPrevTest(string input, int expected)
    {
        var nums = Util.ExtractNumbers<int>(input).ToArray();
        Assert.AreEqual(expected, Day09.GetPrev(nums));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Oasis_Part1_Regression()
    {
        Assert.AreEqual(1868368343, Day09.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Oasis_Part2_Regression()
    {
        Assert.AreEqual(1022, Day09.Part2(input));
    }
}

