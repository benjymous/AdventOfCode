using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day02Test
{
    readonly string input = Util.GetInput<Day02>();

    readonly string test =
@"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void Cubes01Test()
    {
        Assert.AreEqual(8, Day02.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Cubes02Test()
    {
        Assert.AreEqual(2286, Day02.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void CubeConundrum_Part1_Regression()
    {
        Assert.AreEqual(2237, Day02.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void CubeConundrum_Part2_Regression()
    {
        Assert.AreEqual(66681, Day02.Part2(input));
    }
}

