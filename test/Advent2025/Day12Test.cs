using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2025.Test;

[TestCategory("2025")]
[TestClass]
public class Day12Test
{
    readonly string input = Util.GetInput<Day12>();

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Presentris_Part1_Regression()
    {
        Assert.AreEqual(534, Day12.Part1(input));
    }
}