using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXVIII.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day08Test
    {
        string input = Util.GetInput<Day08>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Metadata_Part1_Regression()
        {
            Assert.AreEqual(42501, Day08.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Metadata_Part2_Regression()
        {
            Assert.AreEqual(30857, Day08.Part2(input));
        }

    }
}
