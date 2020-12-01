using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXVII.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day01Test
    {
        string input = Util.GetInput<Day01>();

        [TestCategory("Test")]
        [DataRow("1122", 3)]
        [DataRow("1111", 4)]
        [DataRow("1234", 0)]
        [DataRow("91212129", 9)]
        [DataTestMethod]
        public void Captcha01Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVII.Day01.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("1212", 6)]
        [DataRow("1221", 0)]
        [DataRow("123425", 4)]
        [DataRow("123123", 12)]
        [DataRow("12131415", 4)]
        [DataTestMethod]
        public void Captcha02Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVII.Day01.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Captcha_Part1_Regression()
        {
            Assert.AreEqual(1141, MMXVII.Day01.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Captcha_Part2_Regression()
        {
            Assert.AreEqual(950, MMXVII.Day01.Part2(input));
        }

    }
}
