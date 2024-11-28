using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day15Test
{
    readonly string input = Util.GetInput<Day15>();
    readonly string test = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";

    [TestCategory("Test")]
    [DataRow("rn=1", 30)]
    [DataRow("cm-", 253)]
    [DataRow("qp=3", 97)]
    [DataRow("cm=2", 47)]
    [DataRow("qp-", 14)]
    [DataRow("pc=4", 180)]
    [DataRow("ot=9", 9)]
    [DataRow("ab=5", 197)]
    [DataRow("pc-", 48)]
    [DataRow("pc=6", 214)]
    [DataRow("ot=7", 231)]
    [DataTestMethod]
    public void HashTest(string input, int expected)
    {
        Assert.AreEqual(expected, Day15.CalculateHASH(input));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Hashing01Test()
    {
        Assert.AreEqual(1320, Day15.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void LensBoxTest()
    {
        var Box = new Day15.LensBox(0);

        var Lens1 = (Label: "A", FocalLength: 1);
        var Lens2 = (Label: "B", FocalLength: 2);
        var Lens3 = (Label: "A", FocalLength: 3);

        Box.AddLens(Lens1);
        Box.AddLens(Lens2);
        Box.AddLens(Lens3);

        Assert.AreEqual(2, Box.Count);
        Assert.AreEqual("A", Box[0].Label);
        Assert.AreEqual(3, Box[0].FocalLength);
        Assert.AreEqual("B", Box[1].Label);
        Assert.AreEqual(2, Box[1].FocalLength);

    }

    [TestCategory("Test")]
    [TestMethod]
    public void Boxing02Test()
    {
        Assert.AreEqual(145, Day15.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void LensLibrary_Part1_Regression()
    {
        Assert.AreEqual(507666, Day15.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void LensLibrary_Part2_Regression()
    {
        Assert.AreEqual(233537, Day15.Part2(input));
    }
}

