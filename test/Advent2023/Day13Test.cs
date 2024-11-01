using AoC.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day13Test
{
    readonly string input = Util.GetInput<Day13>();

    readonly string test = @"#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#".Replace("\r", "");

    readonly string test1 = @"#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void FindColMirrorTest()
    {
        Assert.AreEqual(5, Day13.FindMirror(Util.ParseMatrix<char>(test1).Columns(), QuestionPart.Part1));
    }

    readonly string test2 = @"#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void FindRowMirrorTest()
    {
        Assert.AreEqual(4, Day13.FindMirror(Util.ParseMatrix<char>(test2).Rows(), QuestionPart.Part1));
    }

    /*

    #....#.##   1
    ...##.#.#   2
    ###..#...   3
    #....#.##   4
    ######...   5
    ..##.##..   6
    ..#...#.#   7
    ..#...#.#   8
    ..##.##..   9
    ######...   10
    #....#.##   11
    ###..#...   12
    ...##.#.#   13
    #...##.## v 14
    #...##.## ^ 15

    */

    const string test3 = "#....#.##\n...##.#.#\n###..#...\n#....#.##\n######...\n..##.##..\n..#...#.#\n..#...#.#\n..##.##..\n######...\n#....#.##\n###..#...\n...##.#.#\n#...##.##\n#...##.##";

    /*
    
    #..###.#.
    ...#...##
    ##...##..
    ###.###..
    ##...#.##
    ...###.##
    ...#.#.##
    ##...#.##
    ###.###..
    ##...##..
    ...#...##
    #..###.#.
    #..###.#.

    */

    const string test4 = "#..###.#.\n...#...##\n##...##..\n###.###..\n##...#.##\n...###.##\n...#.#.##\n##...#.##\n###.###..\n##...##..\n...#...##\n#..###.#.\n#..###.#.";

    const string test5 = "..##.\n##..#\n#...#\n..##.\n..##.";

    /*


..##.
##..#
#...#
..##.
..##.

    */

    [TestCategory("Test")]
    [DataRow(test3, 1400)]
    [DataRow(test4, 1200)]
    [DataRow(test5, 400)]
    [DataTestMethod]
    public void GetScoreTest(string input, int expected)
    {
        Assert.AreEqual(expected, Day13.GetMirrorScore(input, QuestionPart.Part1));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void FindRowMirrorTestPt2()
    {
        Assert.AreEqual(3, Day13.FindMirror(Util.ParseMatrix<char>(test1).Rows(), QuestionPart.Part2));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Mirroring_01Test()
    {
        Assert.AreEqual(405, Day13.Part1(test));
    }

    [TestCategory("Test")]
    [DataRow("###", "#.#", true)]
    [DataRow(".##", "#.#", false)]
    [DataRow(".##", ".##", true)]
    [DataTestMethod]
    public void DiffByOneTest(string in1, string in2, bool expected)
    {
        Assert.AreEqual(expected, Day13.StringsDifferByNoMoreThanOne(in1, in2));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Mirroring_Part1_Regression()
    {
        Assert.AreEqual(37381, Day13.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Mirroring_Part2_Regression()
    {
        Assert.AreNotEqual(37381, Day13.Part2(input));
    }
}

