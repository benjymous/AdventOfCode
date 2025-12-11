using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2025.Test;

[TestCategory("2025")]
[TestClass]
public class Day11Test
{
    readonly string input = Util.GetInput<Day11>();
    readonly string test1 = "aaa: you hhh\nyou: bbb ccc\nbbb: ddd eee\nccc: ddd eee fff\nddd: ggg\neee: out\nfff: out\nggg: out\nhhh: ccc fff iii\niii: out";
    readonly string test2 = "svr: aaa bbb\naaa: fft\nfft: ccc\nbbb: tty\ntty: ccc\nccc: ddd eee\nddd: hub\nhub: fff\neee: dac\ndac: fff\nfff: ggg hhh\nggg: out\nhhh: out";

    [TestCategory("Test")]
    [TestMethod]
    public void ReactorPath_01Test()
    {
        Assert.AreEqual(5, Day11.Part1(test1));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void ReactorPath_02Test()
    {
        Assert.AreEqual(2, Day11.Part2(test2));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Reactor_Part1_Regression()
    {
        Assert.AreEqual(724, Day11.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Reactor_Part2_Regression()
    {
        Assert.AreEqual(473930047491888, Day11.Part2(input));
    }
}