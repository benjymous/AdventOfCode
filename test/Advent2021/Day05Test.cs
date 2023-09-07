using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestCategory("Lines")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day05Test
    {
        readonly string input = Util.GetInput<Day05>();

        readonly string test = @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";


        [TestCategory("Test")]
        [DataTestMethod]
        public void Vents01Test()
        {
            Assert.AreEqual(5, Day05.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Vents02Test()
        {
            Assert.AreEqual(12, Day05.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Vents_Part1_Regression()
        {
            Assert.AreEqual(5092, Day05.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Vents_Part2_Regression()
        {
            Assert.AreEqual(20484, Day05.Part2(input));
        }
    }
}
