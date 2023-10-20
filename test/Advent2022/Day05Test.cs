using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day05Test
    {
        readonly string input = Util.GetInput<Day05>();

        readonly string test = @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        public void Stacking01Test()
        {
            Assert.AreEqual("CMZ", Day05.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Stacking02Test()
        {
            Assert.AreEqual("MCD", Day05.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Stacking_Part1_Regression()
        {
            Assert.AreEqual("HNSNMTLHQ", Day05.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Stacking_Part2_Regression()
        {
            Assert.AreEqual("RNLFDJMCT", Day05.Part2(input));
        }
    }
}
