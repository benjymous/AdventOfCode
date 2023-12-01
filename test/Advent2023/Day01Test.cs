using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day01Test
{
    readonly string input = Util.GetInput<Day01>();


    [TestCategory("Test")]
    [DataRow("1abc2", 12)]
    [DataRow("pqr3stu8vwx", 38)]
    [DataRow("a1b2c3d4e5f", 15)]
    [DataRow("treb7uchet", 77)]
    [DataRow("hello12there", 12)]
    [DataTestMethod]
    public void Trebuchet01Test(string input, int expected)
    {

        var v = new Day01.Value(input);

        Assert.AreEqual(expected, v.ValPt1);
    }

    [TestCategory("Test")]
    [DataRow("two1nine", 29)]
    [DataRow("eightwothree",83)]
    [DataRow("abcone2threexyz",13)]
    [DataRow("xtwone3four",24)]
    [DataRow("4nineeightseven2",42)]
    [DataRow("zoneight234",14)]
    [DataRow("7pqrstsixteen",76)]
    [DataRow("ninefour", 94)]
    [DataTestMethod]
    public void Trebuchet02Test(string input, int expected)
    {
        var v = new Day01.Value(input);

        Assert.AreEqual(expected, v.ValPt2);
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Trebuchet_Part1_Regression()
    {
        Assert.AreEqual(53334, Day01.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Trebuchet_Part2_Regression()
    {
        Assert.AreEqual(52834, Day01.Part2(input));
    }
}

