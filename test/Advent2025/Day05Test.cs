using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2025.Test;

[TestCategory("2025")]
[TestClass]
public class Day05Test
{
    readonly string input = Util.GetInput<Day05>();
    readonly string test = "3-5\n10-14\n16-20\n12-18\n\n1\n5\n8\n11\n17\n32";

    [TestCategory("Test")]
    [TestMethod]
    public void Freshness_01Test()
    {
        Assert.AreEqual(3, Day05.Part1(test));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void Freshness_02Test()
    {
        Assert.AreEqual(14ul, Day05.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Cafeteria_Part1_Regression()
    {
        Assert.AreEqual(811, Day05.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Cafeteria_Part2_Regression()
    {
        Assert.AreEqual(338189277144473ul, Day05.Part2(input));
    }
}