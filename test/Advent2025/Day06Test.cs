using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2025.Test;

[TestCategory("2025")]
[TestClass]
public class Day06Test
{
    readonly string input = Util.GetInput<Day06>();
    readonly string test = "123 328  51 64 \n 45 64  387 23 \n  6 98  215 314\n*   +   *   +  \n";

    [TestCategory("Test")]
    [TestMethod]
    public void CephalopodMaths_01Test()
    {
        Assert.AreEqual(4277556, Day06.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void CephalopodMaths_02Test()
    {
        Assert.AreEqual(3263827, Day06.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void CephalopodMaths_Part1_Regression()
    {
        Assert.AreEqual(7229350537438, Day06.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void CephalopodMaths_Part2_Regression()
    {
        Assert.AreEqual(11479269003550, Day06.Part2(input));
    }
}