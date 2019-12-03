using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestClass]
    public class Day14Test
    {
        [DataRow(9, 10, "5158916779")]
        [DataRow(5, 10, "0124515891")]
        [DataRow(18, 10, "9251071085")]
        [DataRow(2018, 10, "5941429882")]
        [DataTestMethod]
        public void Chocolate01(int start, int keep, string expected)
        {
            Assert.AreEqual(expected, Day14.Part1(start, keep));                    
        }

        [DataRow("51589", 9)]
        [DataRow("01245", 5)]
        [DataRow("92510", 18)]
        [DataRow("59414", 2018)]
        [DataTestMethod]
        public void Chocolate02(string search, int expected)
        {
            Assert.AreEqual(expected, Day14.Part2(search));                    
        }

        [DataTestMethod]
        public void Chocolate_Part1_Regression()
        {
            var d = new Day14();
            var input = Util.GetInput(d);
            Assert.AreEqual("4910101614", Day14.Part1(int.Parse(input), 10));
        }

        [DataTestMethod]
        public void Chocolate_Part2_Regression()
        {
            var d = new Day14();
            var input = Util.GetInput(d);
            Assert.AreEqual(20253137, Day14.Part2(input));
        }
     
    }
}
