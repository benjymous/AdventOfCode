using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestCategory("ManhattanVector")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day10Test
    {
        readonly string input = Util.GetInput<Day10>();

        [TestCategory("Regression")]
        [TestCategory("OCR")]
        [DataTestMethod]
        public void StarSigns_Part1_Regression()
        {
            Assert.AreEqual("FBHKLEAG", Utils.OCR.TextReader.Read(Day10.Part1(input, new ConsoleOut())));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void StarSigns_Part2_Regression()
        {
            Assert.AreEqual(10009, Day10.Part2(input));
        }
    }
}
