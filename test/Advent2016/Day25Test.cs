using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestCategory("BunnyCPU")]
    [TestClass]
    public class Day25Test
    {
        readonly string input = Util.GetInput<Day25>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Signal_Part1_Regression()
        {
            Assert.AreEqual(189, Day25.Part1(input));
        }
    }
}
