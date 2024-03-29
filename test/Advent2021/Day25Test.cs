using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestCategory("PackedVect")]
    [TestClass]
    public class Day25Test
    {
        readonly string input = Util.GetInput<Day25>();

        readonly string test = @"v...>>.vv>
.vv>>.vv..
>>.>v>...v
>>v>>.>.v.
v>v.vv.v..
>.>>..v...
.vv..>.>v.
v.v..>>v.v
....v..v.>".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        public void CucumberTest()
        {
            Assert.AreEqual(58, Day25.Part1(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Cucumber_Part1_Regression()
        {
            Assert.AreEqual(579, Day25.Part1(input));
        }
    }
}
