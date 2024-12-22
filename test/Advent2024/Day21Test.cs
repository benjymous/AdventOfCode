using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day21Test
{
    readonly string input = Util.GetInput<Day21>();
    readonly string test = "029A\n980A\n179A\n456A\n379A";

    [TestCategory("Test")]
    [DataTestMethod]
    public void _01Test()
    {
        Assert.AreEqual(126384, Day21.Part1(test));
    }

    [TestCategory("Test")]

    [DataRow("029A", 68 * 29)]
    [DataRow("980A", 60 * 980)]
    [DataRow("179A", 68 * 179)]
    [DataRow("456A", 64 * 456)]
    [DataRow("379A", 64 * 379)]

    [DataRow("879A", 70 * 879)]
    [DataRow("682A", 68 * 682)]
    [DataRow("413A", 70 * 413)]
    [DataRow("480A", 74 * 480)]
    [DataRow("083A", 66 * 83)]

    [DataTestMethod]
    public void _01aTest(string input, int expected)
    {
        Assert.AreEqual(expected, Day21.GetScore(input));
    }

    [TestCategory("Test")]
    [DataRow("???", 0)]
    [DataTestMethod]
    public void _02Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day21.Part2(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part1_Regression()
    {
        Assert.AreEqual(177814, Day21.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part2_Regression()
    {
        Assert.AreEqual(220493992841852, Day21.Part2(input));
    }
}