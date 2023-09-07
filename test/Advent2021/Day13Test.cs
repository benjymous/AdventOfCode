using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day13Test
    {
        readonly string input = Util.GetInput<Day13>();

        readonly string test = @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Origami01Test()
        {
            Assert.AreEqual(17, Day13.Part1(test.Replace("\r", "")));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Origami02Test()
        {
            Assert.AreEqual("243C751E061AA9A187EDACEE48B7965C", Day13.Part2(test.Replace("\r", ""), new ConsoleOut()));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Origami_Part1_Regression()
        {
            Assert.AreEqual(710, Day13.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Origami_Part2_Regression()
        {
            Assert.AreEqual("64E2320B4AC529E90D02EF51B3B4B7CE", Day13.Part2(input, new ConsoleOut()));
        }
    }
}
