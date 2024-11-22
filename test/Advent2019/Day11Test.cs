using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestCategory("ManhattanVector")]
    [TestCategory("Direction")]
    [TestClass]
    public class Day11Test
    {
        readonly string input = Util.GetInput<Day11>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Painter_Part1_Regression()
        {
            Assert.AreEqual(2018, Day11.Part1(input));
        }

        [TestCategory("Regression")]
        [TestCategory("OCR")]
        [DataTestMethod]
        public void Painter_Part2_Regression()
        {
            Assert.AreEqual("APFKRKBR", Day11.Part2(input, new ConsoleOut()));
        }
    }
}
