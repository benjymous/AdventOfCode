using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day25Test
    {
        string input = Util.GetInput<Day25>();
        string test = "5764801\n17807724";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Encryption1Test()
        {
            Assert.AreEqual(14897079, Day25.Part1(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Combo_Part1_Regression()
        {
            Assert.AreEqual(10187657, Day25.Part1(input));
        }

    }
}
