using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestCategory("PackedString")]
[TestClass]
public class Day20Test
{
    readonly string input = Util.GetInput<Day20>();
    readonly string test = @"broadcaster -> a
%a -> inv, con
&inv -> b
%b -> con
&con -> output".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void Signals_01Test()
    {
        Assert.AreEqual(11687500, Day20.Part1(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void PulsePropogation_Part1_Regression()
    {
        Assert.AreEqual(919383692, Day20.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void PulsePropogation_Part2_Regression()
    {
        Assert.AreEqual(247702167614647, Day20.Part2(input));
    }
}

