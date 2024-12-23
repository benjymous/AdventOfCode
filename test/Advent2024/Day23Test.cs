using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day23Test
{
    readonly string input = Util.GetInput<Day23>();
    readonly string test = "kh-tc\nqp-kh\nde-cg\nka-co\nyn-aq\nqp-ub\ncg-tb\nvc-aq\ntb-ka\nwh-tc\nyn-cg\nkh-ub\nta-co\nde-co\ntc-td\ntb-wq\nwh-td\nta-ka\ntd-qp\naq-cg\nwq-ub\nub-vc\nde-ta\nwq-aq\nwq-vc\nwh-yn\nka-de\nkh-ta\nco-tc\nwh-qp\ntb-vc\ntd-yn";

    [TestCategory("Test")]
    [DataTestMethod]
    public void _01Test()
    {
        Assert.AreEqual(7, Day23.Part1(test));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void _02Test()
    {
        Assert.AreEqual("co,de,ka,ta", Day23.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part1_Regression()
    {
        Assert.AreEqual(1054, Day23.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void _Part2_Regression()
    {
        Assert.AreEqual("ch,cz,di,gb,ht,ku,lu,tw,vf,vt,wo,xz,zk", Day23.Part2(input));
    }
}