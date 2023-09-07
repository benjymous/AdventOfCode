using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day01Test
    {
        readonly string input = Util.GetInput<Day01>();

        [TestCategory("Test")]
        [DataRow("+1", 1)]
        [DataRow("+1\n+2\n", 3)]
        [DataRow("+1, -2, +3, +1", 3)]
        [DataRow("+1, +1, +1", 3)]
        [DataRow("+1, +1, -2", 0)]
        [DataRow("-1, -2, -3", -6)]
        [DataTestMethod]
        public void Callibrate01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day01.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("+1, -1", 0)]
        [DataRow("+3, +3, +4, -2, -4", 10)]
        [DataRow("-6, +3, +8, +5, -6", 5)]
        [DataRow("+7, +7, -2, -7, -4", 14)]
        [DataTestMethod]
        public void Callibrate02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day01.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Callibration_Part1_Regression()
        {
            Assert.AreEqual(553, Day01.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Callibration_Part2_Regression()
        {
            Assert.AreEqual(78724, Day01.Part2(input));
        }

    }
}
