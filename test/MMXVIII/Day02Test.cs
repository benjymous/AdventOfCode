using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestClass]
    public class Day02Test
    {
        [DataRow("abcdef,bababc,abbcde,abcccd,aabcdd,abcdee,ababab", 12)]
        [DataTestMethod]
        public void Inventory01Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day02.Part1(input));
        }

        [DataRow("abcde,fghij,klmno,pqrst,fguij,axcye,wvxyz", "fgij")]
        [DataTestMethod]
        public void Inventory02Test(string input, string expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day02.Part2(input));
        }

        [DataTestMethod]
        public void Inventory_Part1_Regression()
        {
            var d = new Day02();
            var input = Util.GetInput(d);
            Assert.AreEqual(9139, Day02.Part1(input));
        }

        [DataTestMethod]
        public void Inventory_Part2_Regression()
        {
            var d = new Day02();
            var input = Util.GetInput(d);
            Assert.AreEqual("uqcidadzwtnhsljvxyobmkfyr", Day02.Part2(input));
        }

    }
}
