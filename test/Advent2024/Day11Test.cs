using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day11Test
{
    readonly string input = Util.GetInput<Day11>();
    readonly string test = "125 17";

    [TestCategory("Test")]
    [TestMethod]
    public void Pebbles_01Test()
    {
        Assert.AreEqual(55312, Day11.Part1(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void PebbleCount_Part1_Regression()
    {
        Assert.AreEqual(217443, Day11.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void PebbleCount_Part2_Regression()
    {
        Assert.AreEqual(257246536026785, Day11.Part2(input));
    }
}