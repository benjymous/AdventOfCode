using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestCategory("2019")]
    [TestClass]
    public class Day24Test
    {
        string input = Util.GetInput<Day24>();

        [TestCategory("Test")]
        [DataRow("....#\n#..#.\n#..##\n..#..\n#....",2129920)]
        [DataTestMethod]
        public void BiodiversityTest(string input, int expected)
        {
            Assert.AreEqual(expected, Day24.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("....#\n#..#.\n#..##\n..#..\n#....",99)]
        [DataTestMethod]
        public void InfiniteTest(string input, int expected)
        {
            Assert.AreEqual(expected, Day24.Part2(input,10));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Bugs_Part1_Regression()
        {
            Assert.AreEqual(18852849, Day24.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Bugs_Part2_Regression()
        {
            Assert.AreEqual(1948, Day24.Part2(input));
        }
    }
}
