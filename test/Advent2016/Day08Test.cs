using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day08Test
    {
        readonly string input = Util.GetInput<Day08>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void TFA_Part1_Regression()
        {
            Assert.AreEqual(123, Day08.Part1(input));
        }

        [TestCategory("Regression")]
        [TestCategory("OCR")]
        [DataTestMethod]
        public void TFA_Part2_Regression()
        {
            Assert.AreEqual("AFBUPZBJPS", Utils.OCR.TextReader.Read(Day08.Part2(input, new ConsoleOut())));
        }
    }
}
