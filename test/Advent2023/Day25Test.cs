using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day25Test
{
    readonly string input = Util.GetInput<Day25>();

    readonly string test = @"jqt: rhn xhk nvd
rsh: frs pzl lsr
xhk: hfx
cmg: qnr nvd lhk bvb
rhn: xhk bvb hfx
bvb: xhk hfx
pzl: lsr hfx nvd
qnr: nvd
ntq: jqt hfx bvb xhk
nvd: lhk
lsr: lhk
rzs: qnr cmg lsr rsh
frs: qnr lhk lsr".Replace("\r", "");

    [TestCategory("Test")]
    [TestMethod]
    public void Connections_01Test()
    {
        Assert.AreEqual(54, Day25.Part1(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Connections_Part1_Regression()
    {
        Assert.AreEqual(602151, Day25.Part1(input));
    }
}

