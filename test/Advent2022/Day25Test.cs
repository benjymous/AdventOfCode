using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day25Test
    {
        readonly string input = Util.GetInput<Day25>();

        public static long SnafuToDecimal(string snafu) => new Snafu(snafu).ToDecimal();

        public static string DecimalToSnafu(long value) => new Snafu(value).ToString();

        [TestCategory("Test")]
        [DataRow(1, "1")]
        [DataRow(2, "2")]
        [DataRow(3, "1=")]
        [DataRow(4, "1-")]
        [DataRow(5, "10")]
        [DataRow(6, "11")]
        [DataRow(7, "12")]
        [DataRow(8, "2=")]
        [DataRow(9, "2-")]
        [DataRow(10, "20")]
        [DataRow(15, "1=0")]
        [DataRow(20, "1-0")]
        [DataRow(2022, "1=11-2")]
        [DataRow(12345, "1-0---0")]
        [DataRow(314159265, "1121-1110-1=0")]
        [DataTestMethod]
        public void DecimalToSnafuTest(int input, string expected)
        {
            Assert.AreEqual(expected, DecimalToSnafu(input));
        }

        [TestCategory("Test")]
        [DataRow("1121-1110-1=0", 314159265)]
        [DataRow("1-0---0", 12345)]
        [DataRow("1=-0-2", 1747)]
        [DataRow("12111", 906)]
        [DataRow("2=0=", 198)]
        [DataRow("21", 11)]
        [DataRow("2=01", 201)]
        [DataRow("111", 31)]
        [DataRow("20012", 1257)]
        [DataRow("112", 32)]
        [DataRow("1=-1=", 353)]
        [DataRow("1-12", 107)]
        [DataRow("12", 7)]
        [DataRow("1=", 3)]
        [DataRow("122", 37)]
        [DataTestMethod]
        public void SnafuToDecimalTest(string input, int expected)
        {
            Assert.AreEqual(expected, SnafuToDecimal(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Snafu_Part1_Regression()
        {
            Assert.AreEqual("20=2-02-0---02=22=21", Day25.Part1(input).ToString());
        }
    }
}
