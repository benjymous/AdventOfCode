using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day08Test
    {
        readonly string input = Util.GetInput<Day08>();

        [TestCategory("Test")]
        [DataRow("2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2", 138)]
        [DataTestMethod]
        public void MetadataTest(string dataRow, int expected)
        {
            Assert.AreEqual(expected, Day08.Part1(dataRow));
        }

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
