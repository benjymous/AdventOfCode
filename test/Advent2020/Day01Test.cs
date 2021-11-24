using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day01Test
    {
        string input = Util.GetInput<Day01>();

        [TestCategory("Test")]
        [DataRow("1721\n979\n366\n299\n675\n1456", 514579)]
        [DataTestMethod]
        public void Expenses01Test(string input, int expected)
        {
            Assert.IsTrue(Advent2020.Day01.Part1(input) == expected);
        }

        [TestCategory("Test")]
        [DataRow("1721\n979\n366\n299\n675\n1456", 241861950)]
        [DataTestMethod]
        public void Expenses02Test(string input, int expected)
        {
            Assert.IsTrue(Advent2020.Day01.Part2(input) == expected);
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Expenses_Part1_Regression()
        {
            Assert.AreEqual(788739, Day01.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Expenses_Part2_Regression()
        {
            Assert.AreEqual(178724430, Day01.Part2(input));
        }
    }
}
