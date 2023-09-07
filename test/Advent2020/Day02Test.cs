using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day02Test
    {

        readonly string input = Util.GetInput<Day02>();

        [TestCategory("Test")]
        [DataRow("1-3 a: abcde\n1-3 b: cdefg\n2-9 c: ccccccccc", 2)]
        [DataTestMethod]
        public void Password01Test(string input, int expected)
        {
            Assert.IsTrue(Day02.Part1(input) == expected);
        }

        [TestCategory("Test")]
        [DataRow("1-3 a: abcde\n1-3 b: cdefg\n2-9 c: ccccccccc", 1)]
        [DataTestMethod]
        public void Password02Test(string input, int expected)
        {
            Assert.IsTrue(Day02.Part2(input) == expected);
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Passwords_Part1_Regression()
        {
            Assert.AreEqual(524, Day02.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Passwords_Part2_Regression()
        {
            Assert.AreEqual(485, Day02.Part2(input));
        }
    }
}
