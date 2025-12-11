using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2025.Test;

[TestCategory("2025")]
[TestClass]
public class Day10Test
{
    readonly string input = Util.GetInput<Day10>();
    readonly string test = "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}\n[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}\n[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}";

    [TestCategory("Test")]
    [TestMethod]
    public void ButtonCombos01Test()
    {
        Assert.AreEqual(7, Day10.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void ButtonCombos02Test()
    {
        Assert.AreEqual(33, Day10.Part2(test));
    }

    [TestCategory("Test")]
    [DataTestMethod]

    [DataRow("[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}", 10)]
    [DataRow("[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}", 12)]
    [DataRow("[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}", 11)]

    [DataRow("[#.##] (3) (1,3) (0,3) (0,2) (0,1,3) (0) {42,36,8,56}", 64)]
    [DataRow("[#.#.#] (0,1,2) (1,3,4) (0,1) (1,3) (2,4) (0,1,4) (1,2,4) {28,50,33,6,46}", 57)]

    [DataRow("[#.....#..#] (1,3,4,6,8) (0,2,3,4,5,7,9) (2,3,4,8) (0,4,8) (5,7) (6,7) (0,1,3,4,6,7,8,9) (0,1,2,4,5,7,8) {34,38,37,53,64,18,40,47,57,23}", 77)]

    [DataRow("[..#..] (1,4) (0,3) (2) {4,11,19,4,11}", 34)]
    public void Solver02Test(string input, int expected)
    {
        Assert.AreEqual(expected, Day10.Part2(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Factory_Part1_Regression()
    {
        Assert.AreEqual(484, Day10.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Factory_Part2_Regression()
    {
        Assert.AreEqual(19210, Day10.Part2(input));
    }
}