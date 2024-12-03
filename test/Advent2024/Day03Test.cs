using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day03Test
{
    readonly string input = Util.GetInput<Day03>();
    readonly string test1 = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
    readonly string test2 = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

    [TestCategory("Test")]
    [TestMethod]
    public void Multiplication_01Test()
    {
        Assert.AreEqual(161, Day03.Part1(test1));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Multiplication_02Test()
    {
        Assert.AreEqual(48, Day03.Part2(test2));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Muls_Part1_Regression()
    {
        Assert.AreEqual(157621318, Day03.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Muls_Part2_Regression()
    {
        Assert.AreEqual(79845780, Day03.Part2(input));
    }
}