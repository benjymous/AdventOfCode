using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day14Test
    {
        readonly string input = Util.GetInput<Day14>();

        [TestCategory("Test")]
        [DataRow(9, 10, "5158916779")]
        [DataRow(5, 10, "0124515891")]
        [DataRow(18, 10, "9251071085")]
        [DataRow(2018, 10, "5941429882")]
        [DataTestMethod]
        public void Chocolate01(int start, int keep, string expected)
        {
            Assert.AreEqual(expected, Day14.Part1(start, keep));
        }

        [TestCategory("Test")]
        [DataRow("51589", 9)]
        [DataRow("01245", 5)]
        [DataRow("92510", 18)]
        [DataRow("59414", 2018)]
        [DataTestMethod]
        public void Chocolate02(string search, int expected)
        {
            Assert.AreEqual(expected, Day14.Part2(search));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Chocolate_Part1_Regression()
        {
            Assert.AreEqual("4910101614", Day14.Part1(int.Parse(input), 10));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Chocolate_Part2_Regression()
        {
            Assert.AreEqual(20253137, Day14.Part2(input));
        }
    }
}
