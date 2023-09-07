using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestClass]
    public class Day04Test
    {
        readonly string input = Util.GetInput<Day04>();

        static int[] ToArray(string input)
        {
            return input.Select(x => x-'0').ToArray();
        }

        [TestCategory("Test")]
        [DataRow("111111", true)]
        //[DataRow("223450", false)] // checked by outer code
        [DataRow("123789", false)]
        [DataTestMethod]
        public void SecureTest01(string input, bool expected)
        {
            Assert.AreEqual(expected, Day04.HasAdjacentPair(ToArray(input), false));
        }

        [TestCategory("Test")]
        [DataRow("112233", true)]
        [DataRow("123444", false)]
        [DataRow("111122", true)]
        [DataTestMethod]
        public void SecureTest02(string input, bool expected)
        {
            Assert.AreEqual(expected, Day04.HasAdjacentPair(ToArray(input), true));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Secure_Part1_Regression()
        {
            Assert.AreEqual(1246, Day04.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Secure_Part2_Regression()
        {
            Assert.AreEqual(814, Day04.Part2(input));
        }

    }
}
