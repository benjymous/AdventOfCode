using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day14Test
    {
        readonly string input = Util.GetInput<Day14>();

        readonly string test = @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Polimer01Test()
        {
            Assert.AreEqual(1588, Day14.Part1(test.Replace("\r", "")));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Polimer02Test()
        {
            Assert.AreEqual(2188189693529, Day14.Part2(test.Replace("\r", "")));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Polimer_Part1_Regression()
        {
            Assert.AreEqual(2947, Day14.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Polimer_Part2_Regression()
        {
            Assert.AreEqual(3232426226464, Day14.Part2(input));
        }
    }
}
