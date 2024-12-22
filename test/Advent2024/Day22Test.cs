﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day22Test
{
    readonly string input = Util.GetInput<Day22>();
    readonly string test1 = "1\n10\n100\n2024";
    readonly string test2 = "1\n2\n3\n2024";

    [TestCategory("Test")]
    [DataRow(100000000, 16113920)]
    [DataTestMethod]
    public void PruneTest(int secret, int expected)
    {
        Assert.AreEqual(expected, Day22.Prune(secret));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void _01Test()
    {
        Assert.AreEqual(37327623, Day22.Part1(test1));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void _02Test()
    {
        Assert.AreEqual(23, Day22.Part2(test2));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part1_Regression()
    {
        Assert.AreEqual(13429191512, Day22.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part2_Regression()
    {
        Assert.AreEqual(1582, Day22.Part2(input));
    }
}