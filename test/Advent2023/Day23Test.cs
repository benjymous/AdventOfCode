﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day23Test
{
    readonly string input = Util.GetInput<Day23>();
    readonly string test = @"#.#####################
#.......#########...###
#######.#########.#.###
###.....#.>.>.###.#.###
###v#####.#v#.###.#.###
###.>...#.#.#.....#...#
###v###.#.#.#########.#
###...#.#.#.......#...#
#####.#.#.#######.#.###
#.....#.#.#.......#...#
#.#####.#.#.#########v#
#.#...#...#...###...>.#
#.#.#v#######v###.###v#
#...#.>.#...>.>.#.###.#
#####v#.#.###v#.#.###.#
#.....#...#...#.#.#...#
#.#########.###.#.#.###
#...###...#...#...#.###
###.###.#.###v#####v###
#...#...#.#.>.>.#.>.###
#.###.###.#.###.#.#v###
#.....###...###...#...#
#####################.#";

    [TestCategory("Test")]
    [TestMethod]
    public void ScenicRoute_01Test()
    {
        Assert.AreEqual(94, Day23.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void ScenicRoute_02Test()
    {
        Assert.AreEqual(154, Day23.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void ALongWalk_Part1_Regression()
    {
        Assert.AreEqual(2070, Day23.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void ALongWalk_Part2_Regression()
    {
        Assert.AreEqual(6498, Day23.Part2(input));
    }
}

