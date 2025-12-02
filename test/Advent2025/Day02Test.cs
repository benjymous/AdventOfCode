using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2025.Test;

[TestCategory("2025")]
[TestClass]
public class Day02Test
{
    readonly string input = Util.GetInput<Day02>();
    readonly string test = @"11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";

    [TestCategory("Test")]
    [TestMethod]
    public void InvalidCodes_01Test()
    {
        Assert.AreEqual(1227775554, Day02.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void InvalidCodes_02Test()
    {
        Assert.AreEqual(4174379265, Day02.Part2(test));
    }

    [TestCategory("Test")]
    [DataRow(12, true, false)]
    [DataRow(11, true, true)]
    [DataRow(111, true, true)]
    [DataRow(1111, true, true)]
    [DataRow(1212, true, true)]
    [DataRow(123123, true, true)]
    [DataRow(123123123, true, true)]
    [DataTestMethod]
    public void Validity02Test(long input, bool part2, bool expected)
    {
        Assert.AreEqual(expected, Day02.Range.IsInvalid(input, part2 ? QuestionPart.Part2 : QuestionPart.Part1));
    }

    [TestCategory("Regression")]
    [TestMethod]
    public void GiftShop_Part1_Regression()
    {
        Assert.AreEqual(37314786486, Day02.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void GiftShop_Part2_Regression()
    {
        Assert.AreEqual(47477053982, Day02.Part2(input));
    }
}