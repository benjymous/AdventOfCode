﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day06Test
{
    readonly string input = ""; // Util.GetInput<Day06>();

    [TestCategory("Test")]
    [DataRow("???", 0)]
    [DataTestMethod]
    public void _01Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day06.Part1(input));
    }

    [TestCategory("Test")]
    [DataRow("???", 0)]
    [DataTestMethod]
    public void _02Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day06.Part2(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part1_Regression()
    {
        Assert.AreEqual(0, Day06.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part2_Regression()
    {
        Assert.AreEqual(0, Day06.Part2(input));
    }
}

