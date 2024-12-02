using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day02Test
{
    readonly string input = Util.GetInput<Day02>();

    readonly string test = "7 6 4 2 1\n1 2 7 8 9\n9 7 6 2 1\n1 3 2 4 5\n8 6 4 4 1\n1 3 6 7 9";

    [TestCategory("Test")]
    [TestMethod]
    public void DeerReport_01Test()
    {
        Assert.AreEqual(2, Day02.Part1(test));
    }

    [TestCategory("Test")]
    [DataRow("7 6 4 2 1", false, true)]
    [DataRow("1 2 7 8 9", false, false)]
    [DataRow("9 7 6 2 1", false, false)]
    [DataRow("1 3 2 4 5", false, false)]
    [DataRow("8 6 4 4 1", false, false)]
    [DataRow("1 3 6 7 9", true, false)]
    [DataTestMethod]
    public void DeerReport_01aTest(string input, bool increasing, bool decreasing)
    {
        var vals = Util.ExtractNumbers(input);
        var inc = Day02.AllIncrease(vals);
        var dec = Day02.AllDecrease(vals);
        Assert.AreEqual(increasing, inc);
        Assert.AreEqual(decreasing, dec);
    }

    [TestCategory("Test")]
    [TestMethod]
    public void DeerReport_02Test()
    {
        Assert.AreEqual(4, Day02.Part2(test));
    }

    [TestCategory("Test")]
    [DataRow("7 6 4 2 1", true)]
    [DataRow("1 2 7 8 9", false)]
    [DataRow("9 7 6 2 1", false)]
    [DataRow("1 3 2 4 5", true)]
    [DataRow("8 6 4 4 1", true)]
    [DataRow("1 3 6 7 9", true)]
    [DataTestMethod]
    public void DeerReport_02aTest(string input, bool isSafe)
    {
        var vals = Util.ExtractNumbers(input);
        Assert.AreEqual(isSafe, Day02.SafeIfDampened(vals));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void DeerReport_Part1_Regression()
    {
        Assert.AreEqual(341, Day02.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void DeerReport_Part2_Regression()
    {
        Assert.AreEqual(404, Day02.Part2(input));
    }
}