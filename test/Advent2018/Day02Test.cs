using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day02Test
    {
        readonly string input = Util.GetInput<Day02>();

        [TestCategory("Test")]
        [DataRow("abcdef,bababc,abbcde,abcccd,aabcdd,abcdee,ababab", 12)]
        [DataTestMethod]
        public void Inventory01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day02.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("abcde,fghij,klmno,pqrst,fguij,axcye,wvxyz", "fgij")]
        [DataTestMethod]
        public void Inventory02Test(string input, string expected)
        {
            Assert.AreEqual(expected, Day02.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Inventory_Part1_Regression()
        {
            Assert.AreEqual(9139, Day02.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Inventory_Part2_Regression()
        {
            Assert.AreEqual("uqcidadzwtnhsljvxyobmkfyr", Day02.Part2(input));
        }
    }
}
