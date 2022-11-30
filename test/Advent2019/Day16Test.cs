using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestClass]
    public class Day16Test
    {
        readonly string input = Util.GetInput<Day16>();

        [TestCategory("Test")]
        [DataRow("80871224585914546619083218645595", "24176176")]
        [DataRow("19617804207202209144916044189917", "73745418")]
        [DataRow("69317163492948606335995924319873", "52432133")]
        [DataTestMethod]
        public void FFTTest(string input, string expected)
        {
            Assert.AreEqual(expected, Day16.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("03036732577212944063491565474664", "84462026")]
        [DataRow("02935109699940807407585447034323", "78725270")]
        [DataRow("03081770884921959731165446850517", "53553731")]
        [DataTestMethod]
        public void FF2Test(string input, string expected)
        {
            Assert.AreEqual(expected, Day16.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void FFT_Part1_Regression()
        {
            Assert.AreEqual("76795888", Day16.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void FFT_Part2_Regression()
        {
            Assert.AreEqual("84024125", Day16.Part2(input));
        }
    }
}
