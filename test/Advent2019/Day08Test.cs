using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestClass]
    public class Day08Test
    {
        readonly string input = Util.GetInput<Day08>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Image_Part1_Regression()
        {
            Assert.AreEqual(1560, Day08.Part1(input));
        }

        [TestCategory("Regression")]
        [TestCategory("OCR")]
        [DataTestMethod]
        public void Image_Part2_Regression()
        {
            Assert.AreEqual("UGCUH", Day08.Part2(input, new ConsoleOut()));
        }
    }
}
