using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2025.Test;

[TestCategory("2025")]
[TestClass]
public class Day08Test
{
    readonly string input = Util.GetInput<Day08>();
    readonly string test = "162,817,812\n57,618,57\n906,360,560\n592,479,940\n352,342,300\n466,668,158\n542,29,236\n431,825,988\n739,650,466\n52,470,668\n216,146,977\n819,987,18\n117,168,530\n805,96,715\n346,949,466\n970,615,88\n941,993,340\n862,61,35\n984,92,344\n425,690,689";

    [TestCategory("Test")]
    [TestMethod]
    public void Circuit_01Test()
    {
        Assert.AreEqual(40, Day08.SolveCircuit(test, 10));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Circuit_02Test()
    {
        Assert.AreEqual(25272, Day08.SolveCircuit(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void LightingCircuit_Part1_Regression()
    {
        Assert.AreEqual(84968, Day08.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void LightingCircuit_Part2_Regression()
    {
        Assert.AreEqual(8663467782, Day08.Part2(input));
    }
}