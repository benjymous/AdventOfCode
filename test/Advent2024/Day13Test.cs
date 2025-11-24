using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day13Test
{
    readonly string input = Util.GetInput<Day13>();

    readonly string test = @"Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279".Replace("\r","");

    [TestCategory("Test")]
    [DataRow(94, 22, 8400, 34, 67, 5400, 80, 40)]
    [DataRow(26, 67, 12748, 66, 21, 12176, 0, 0)]
    [DataRow(17, 84, 7870, 86, 37, 6450, 38, 86)]
    [DataRow(69, 27, 18641, 23, 71, 10279, 0, 0)]
    [DataTestMethod]
    public void TestSolver(int x1, int x2, int x3, int y1, int y2, int y3, int expectedA, int expectedB)
    {
        Assert.AreEqual((expectedA*3) + expectedB, new Day13.ClawMachine(x1, y1, x2, y2, x3, y3).Solve());
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Claw_01Test()
    {
        Assert.AreEqual(480, Day13.Part1(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Claw_Part1_Regression()
    {
        Assert.AreEqual(34393, Day13.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Claw_Part2_Regression()
    {
        Assert.AreEqual(83551068361379, Day13.Part2(input));
    }
}